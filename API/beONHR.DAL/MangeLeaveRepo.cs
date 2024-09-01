using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Email;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface IMangeLeaveRepo
    {
        Task<ClientResponse> applyLeave(ManageLeaveDTO input);
        Task<ClientResponse> GetFilterLeave(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetLeaveById(Guid id);
        Task<ClientResponse> DeleteLeave(Guid id);

    }

    public class MangeLeaveRepo : IMangeLeaveRepo
    {
        private readonly MainContext _context;
        private readonly IEmailRepo _emailRepo;
        private readonly DbContextOptions<MainContext> _contextOptions;

        public MangeLeaveRepo(MainContext context, IEmailRepo emailRepo, DbContextOptions<MainContext> contextOptions)
        {
            _context = context;
            _emailRepo = emailRepo;
            _contextOptions = contextOptions;
        }

        public async Task<ClientResponse> applyLeave(ManageLeaveDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    return await InsertLeave(input);
                }
                else
                {
                    return await UpdateLeave(input);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                return response;
            }
        }

        private async Task<ClientResponse> InsertLeave(ManageLeaveDTO input)
        {
            ClientResponse response = new();

            using (var context = new MainContext(_contextOptions))
            {
                var employeeLeaveBalance = await context.EmployeeYearlyLeaveBalances
                    .Where(x => x.EmployeeId == input.EmployeeId && x.LeaveTypeEmployee == input.Leavetype && !x.IsDeleted)
                    .FirstOrDefaultAsync();

                if (employeeLeaveBalance == null)
                {
                    response.Message = "Leave balance record not found.";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    return response;
                }

                if (employeeLeaveBalance.LeaveQuota != -1 && input.LeaveDay > employeeLeaveBalance.LeaveQuota)
                {
                    response.Message = "Insufficient leave quota.";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    return response;
                }

                if (employeeLeaveBalance.LeaveQuota != -1)
                {
                    employeeLeaveBalance.LeaveQuota -= input.LeaveDay;
                }

                EmployeeLeave model = new EmployeeLeave
                {
                    EmployeeId = input.EmployeeId,
                    Leavetype = input.Leavetype,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    LeaveDay = input.LeaveDay,
                    TeamleadName = "Pending......",
                    OfficeManagementName = "Pending......",
                    leave_duration = input.leave_duration,
                    Leave_Start_From = input.Leave_Start_From,
                    Leave_End = input.Leave_End,
                    Reason = input.Reason,
                    AppliedDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                };

                await context.EmployeeLeaves.AddAsync(model);

                if (employeeLeaveBalance.LeaveQuota != -1)
                {
                    context.EmployeeYearlyLeaveBalances.Update(employeeLeaveBalance);
                }

                var res = await context.SaveChangesAsync();

                if (res == 0)
                {
                    response.Message = "Leave not applied.";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
                    return response;
                }

                var user = await context.Employees
                    .Where(x => x.Id == model.EmployeeId)
                    .Include(x => x.Leader1)
                    .Include(x => x.Leader2)
                    .Include(x => x.DefaulLeader3)
                    .FirstOrDefaultAsync();

                List<string> emails = GetLeaderEmails(user);

                if (emails.Count > 0)
                {
                    EmailMessage options = new EmailMessage
                    {
                        ToEmails = emails,
                        PlaceHolders = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("{{UserName}}", "Team Leader"),
                    new KeyValuePair<string, string>("{{EmployeeName}}", $"{user.FirstName} {user.MiddleName} {user.LastName} ({user.EmployeeNumber})"),
                    new KeyValuePair<string, string>("{{StartDate}}", model.StartDate.ToString()),
                    new KeyValuePair<string, string>("{{EndDate}}", model.EndDate.ToString())
                }
                    };

                    await _emailRepo.SendEmailApplyLeave(options);
                }

                await CreateNotifications(user, model);

                response.Message = "Leave applied successfully.";
                response.HttpResponse = model.Id;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;

                return response;
            }
        }

        private async Task<ClientResponse> UpdateLeave(ManageLeaveDTO input)
        {
            ClientResponse response = new();

            using (var context = new MainContext(_contextOptions))
            {
                var leave = await context.EmployeeLeaves
                    .Where(x => x.Id == input.Id && !x.IsDeleted)
                    .FirstOrDefaultAsync();

                if (leave == null)
                {
                    response.Message = "Leave not applied before.";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
                    return response;
                }

                var employeeLeaveBalance = await context.EmployeeYearlyLeaveBalances
                    .Where(x => x.EmployeeId == input.EmployeeId && x.LeaveTypeEmployee == input.Leavetype && !x.IsDeleted)
                    .FirstOrDefaultAsync();

                if (employeeLeaveBalance == null)
                {
                    response.Message = "Leave balance record not found.";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    return response;
                }

                if (employeeLeaveBalance.LeaveQuota != -1)
                {
                    employeeLeaveBalance.LeaveQuota += leave.LeaveDay;
                }

                if (employeeLeaveBalance.LeaveQuota != -1 && input.LeaveDay > employeeLeaveBalance.LeaveQuota)
                {
                    response.Message = "Insufficient leave quota.";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    return response;
                }

                if (employeeLeaveBalance.LeaveQuota != -1)
                {
                    employeeLeaveBalance.LeaveQuota -= input.LeaveDay;
                }

                leave.EmployeeId = input.EmployeeId;
                leave.Leavetype = input.Leavetype;
                leave.StartDate = input.StartDate;
                leave.EndDate = input.EndDate;
                leave.LeaveDay = input.LeaveDay;
                leave.leave_duration = input.leave_duration;
                leave.Leave_Start_From = input.Leave_Start_From;
                leave.Leave_End = input.Leave_End;
                leave.Reason = input.Reason;
                leave.AppliedDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                context.EmployeeLeaves.Update(leave);

                if (employeeLeaveBalance.LeaveQuota != -1)
                {
                    context.EmployeeYearlyLeaveBalances.Update(employeeLeaveBalance);
                }

                var res = await context.SaveChangesAsync();

                if (res == 0)
                {
                    response.Message = "Leave not updated.";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
                }
                else
                {
                    var user = await context.Employees
                        .Where(x => x.Id == leave.EmployeeId)
                        .Include(x => x.Leader1)
                        .Include(x => x.Leader2)
                        .Include(x => x.DefaulLeader3)
                        .FirstOrDefaultAsync();

                    await UpdateNotifications(user, leave);

                    response.Message = "Leave updated successfully.";
                    response.HttpResponse = leave.Id;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
        }

        private async Task UpdateNotifications(Employee user, EmployeeLeave leave)
        {
            using (var context = new MainContext(_contextOptions))
            {
                var leaderIds = new List<Guid?> { user.Leader1Id, user.Leader2Id, user.DefaulLeaderId };

                foreach (var leaderId in leaderIds)
                {
                    if (leaderId.HasValue)
                    {
                        await UpdateNotificationForLeader(leaderId.Value, user, leave);
                    }
                }
            }
        }

        private async Task UpdateNotificationForLeader(Guid leaderId, Employee user, EmployeeLeave leave)
        {
            using (var context = new MainContext(_contextOptions))
            {
                var notification = await context.Notifications
                    .FirstOrDefaultAsync(n => n.EmployeeId == leaderId && n.NotificationType == leave.Id.ToString());

                if (notification != null)
                {
                    notification.NotificationText = $"Updated leave application from {user.FirstName} {user.LastName} ({user.EmployeeNumber}) | From: {leave.StartDate.ToString("d")} | To: {leave.EndDate.ToString("d")}";

                    await UpdateNotification(new NotificationDTO
                    {
                        NotificationID = notification.NotificationID,
                        EmployeeId = notification.EmployeeId,
                        NotificationText = notification.NotificationText,
                        NotificationType = notification.NotificationType,
                        IsRead = notification.IsRead
                    });
                }
            }
        }

        public async Task<ClientResponse> UpdateNotification(NotificationDTO input)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var existingNotification = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.NotificationID == input.NotificationID);

                if (existingNotification == null)
                {
                    response.Message = "Notification not found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    return response;
                }

                existingNotification.EmployeeId = input.EmployeeId;
                existingNotification.NotificationText = input.NotificationText;
                existingNotification.NotificationType = input.NotificationType;
                existingNotification.IsRead = input.IsRead = false;
                existingNotification.CreatedAt = DateTime.Now;

                _context.Notifications.Update(existingNotification);
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                {
                    response.Message = "Notification not updated";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Notification updated successfully";
                    response.HttpResponse = existingNotification.NotificationID;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                return response;
            }
        }



        private List<string> GetLeaderEmails(Employee user)
        {
            List<string> emails = new List<string>();

            if (user.Leader1Id != null)
            {
                emails.Add(user.Leader1.Email);
            }
            if (user.Leader2Id != null)
            {
                emails.Add(user.Leader2.Email);
            }
            if (user.DefaulLeaderId != null)
            {
                emails.Add(user.DefaulLeader3.Email);
            }

            return emails;
        }

        private async Task CreateNotifications(Employee user, EmployeeLeave model)
        {
            var notifications = new List<Task>();

            if (user.Leader1Id != null)
            {
                notifications.Add(SaveNotification(new NotificationDTO
                {
                    EmployeeId = user.Leader1Id.Value,
                    NotificationText = $"Leave application from {user.FirstName} {user.LastName} - ({user.EmployeeNumber}) | From: {model.StartDate.ToString("d")} | To: {model.EndDate.ToString("d")}",
                    NotificationType = model.Id.ToString(),
                    IsRead = false
                }));
            }
            if (user.Leader2Id != null)
            {
                notifications.Add(SaveNotification(new NotificationDTO
                {
                    EmployeeId = user.Leader2Id.Value,
                    NotificationText = $"Leave application from {user.FirstName} {user.LastName} ({user.EmployeeNumber}) | From: {model.StartDate.ToString("d")} | To: {model.EndDate.ToString("d")}",
                    NotificationType = model.Id.ToString(),
                    IsRead = false
                }));
            }
            if (user.DefaulLeaderId != null)
            {
                notifications.Add(SaveNotification(new NotificationDTO
                {
                    EmployeeId = user.DefaulLeaderId.Value,
                    NotificationText = $"Leave application from {user.FirstName} {user.LastName} ({user.EmployeeNumber}) | From: {model.StartDate.ToString("d")} | To: {model.EndDate.ToString("d")}",
                    NotificationType = model.Id.ToString(),
                    IsRead = false
                }));
            }

            await Task.WhenAll(notifications);
        }

        public async Task<ClientResponse> SaveNotification(NotificationDTO input)
        {
            ClientResponse response = new ClientResponse();

            using (var context = new MainContext(_contextOptions))
            {
                try
                {
                    var notification = new Notification
                    {
                        NotificationID = Guid.NewGuid(),
                        EmployeeId = input.EmployeeId,
                        NotificationText = input.NotificationText,
                        NotificationType = input.NotificationType,
                        IsRead = input.IsRead,
                        CreatedAt = DateTime.Now
                    };

                    await context.Notifications.AddAsync(notification);
                    var result = await context.SaveChangesAsync();

                    if (result == 0)
                    {
                        response.Message = "Notification not saved";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Notification saved successfully";
                        response.HttpResponse = notification.NotificationID;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                catch (Exception ex)
                {
                    response.Message = ex.Message;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.IsSuccess = false;
                }

                return response;
            }
        }



        // *********************** filter Method start  *********************************//
        static Expression<Func<ManageLeaveList, bool>> CombineLambdas(Expression<Func<ManageLeaveList, bool>> expr1, Expression<Func<ManageLeaveList, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(ManageLeaveList));
            Expression body = null;
            if (filterRequset.filterConditionAndOr == (int)filterConditionAndOrEnum.OrCondition)
            {
                body = Expression.OrElse(
                 expr1.Body.ReplaceParameter(expr1.Parameters[0], parameter),
                 expr2.Body.ReplaceParameter(expr2.Parameters[0], parameter)
                );
            }
            else
            {
                body = Expression.AndAlso(
                                expr1.Body.ReplaceParameter(expr1.Parameters[0], parameter),
                                expr2.Body.ReplaceParameter(expr2.Parameters[0], parameter)
                               );
            }

            return Expression.Lambda<Func<ManageLeaveList, bool>>(body, parameter);
        }

        public async Task<ClientResponse> GetFilterLeave(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.EmployeeLeaves.Where(x => x.IsDeleted != true).Include(x => x.LeavetypeId).Include(x => x.Employee).Select(x => new ManageLeaveList
                {
                    Id = x.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Leavetype = x.Leavetype,
                    LeavetypeName = x.LeavetypeId.TypeName,
                    EmployeeId = x.EmployeeId,
                    Leader1 = x.Employee.Leader1Id,
                    Leader2 = x.Employee.Leader2Id,
                    Leader3 = x.Employee.DefaulLeaderId,
                    LeaveDay = x.LeaveDay,
                    leave_duration = x.leave_duration,
                    Leave_Start_From = x.Leave_Start_From,
                    Leave_End = x.Leave_End,
                    Reason = x.Reason,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.MiddleName + " " + x.Employee.LastName,

                }).AsQueryable();
                // Loop through each filter
                if (query.Any())
                {
                    Expression<Func<ManageLeaveList, bool>> combinedCondition = null;

                    var parameter = Expression.Parameter(typeof(ManageLeaveList));
                    Expression<Func<ManageLeaveList, bool>> lambda = null;

                    if (filterRequset.filterModel != null && filterRequset.filterModel.Count() != 0)
                    {
                        foreach (var filter in filterRequset.filterModel)
                        {
                            var filterKey = "";

                            if (filter.Key.IndexOf(".") != -1)
                            {
                                filterKey = filter.Key.Substring(filter.Key.IndexOf(".") + 1).ToString();
                                var filterForignTable = filter.Key.Substring(0, filter.Key.IndexOf(".")).ToString();

                                var ForginTable = Expression.Property(parameter, filterForignTable);
                                var sortTableCol = Expression.Property(ForginTable, filterKey);

                                var value = Expression.Constant(filter.Value.filter);
                                var condition = Expression.Call(sortTableCol, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);

                                var ForginTableInclude = Expression.Lambda(ForginTable, parameter);
                                lambda = Expression.Lambda<Func<ManageLeaveList, bool>>(condition, parameter);
                                if (combinedCondition == null)
                                {
                                    combinedCondition = lambda;
                                }
                                else
                                {
                                    combinedCondition = CombineLambdas(combinedCondition, lambda, filterRequset);

                                }
                                //query = query.Where(lambda);

                            }
                            else
                            {
                                var property = Expression.Property(parameter, filter.Key);
                                if (filter.Value.filterType == "date")
                                {
                                    var date = DateOnly.FromDateTime(DateTime.Parse(filter.Value.dateFrom));
                                    var value = Expression.Constant(date);
                                    var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                                    lambda = Expression.Lambda<Func<ManageLeaveList, bool>>(condition, parameter);
                                }
                                else
                                {

                                    var value = Expression.Constant(filter.Value.filter);
                                    var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                                    lambda = Expression.Lambda<Func<ManageLeaveList, bool>>(condition, parameter);
                                }
                                if (combinedCondition == null)
                                {
                                    combinedCondition = lambda;
                                }
                                else
                                {
                                    combinedCondition = CombineLambdas(combinedCondition, lambda, filterRequset);


                                }
                            }
                        }

                        query = query.Where(combinedCondition);


                    }
                    if (filterRequset.sortModel.colId.IndexOf(".") != -1)
                    {
                        // for Make Key and Table subString
                        var SortKey = filterRequset.sortModel.colId.Substring(filterRequset.sortModel.colId.IndexOf(".") + 1).ToString();
                        var ForignKeyTable = filterRequset.sortModel.colId.Substring(0, filterRequset.sortModel.colId.IndexOf(".")).ToString();

                        //make exprssion
                        var ForignKeyTableColumn = Expression.Property(parameter, ForignKeyTable);

                        var sortTableCol = Expression.Property(ForignKeyTableColumn, SortKey);

                        var ForginTableInclude = Expression.Lambda(sortTableCol, parameter);

                        var sortCodition = Expression.Lambda<Func<ManageLeaveList, string>>(sortTableCol, parameter);

                        switch (filterRequset.sortModel.sortOrder)
                        {
                            case "asc":
                                query = query.OrderBy(sortCodition);

                                break;
                            case "desc":
                                query = query.OrderByDescending(sortCodition);

                                break;
                        };
                    }
                    else
                    {
                        if (filterRequset.sortModel != null)
                        {
                            var SortColumn = Expression.Property(parameter, filterRequset.sortModel.colId);
                            var sortCodition = Expression.Lambda<Func<ManageLeaveList, string>>(SortColumn, parameter);
                            switch (filterRequset.sortModel.sortOrder)
                            {
                                case "asc":
                                    query = query.OrderBy(sortCodition);
                                    break;
                                case "desc":
                                    query = query.OrderByDescending(sortCodition);
                                    break;
                            };
                        }
                        else
                        {

                            query = query;
                        }
                    }
                }
                var totalRecord = query.Count();
                int skip = (int)((filterRequset.PageNumber - 1) * filterRequset.PageSize);
                //int last =
                var leaves = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                responseManageLeave Response = new responseManageLeave()
                {
                    leaves1 = leaves,
                    TotalRecord = totalRecord
                };

                if (leaves == null)
                {
                    response.Message = "No Any Desigantion";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Desigantion Get Sucesfully";
                    response.HttpResponse = Response;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }


                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }



        // *********************** filter Method End  *********************************//


        public async Task<ClientResponse> GetLeaveById(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var designation = await _context.EmployeeLeaves.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (designation != null)
                {

                    response.Message = "Leave Get Sucesfully";
                    response.HttpResponse = designation;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "Leave not Exists";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
                }

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task CreateDeleteNotifications(Employee user, EmployeeLeave leave)
        {
            var notifications = new List<Task>();

            if (user.Leader1Id != null)
            {
                notifications.Add(SaveNotification(new NotificationDTO
                {
                    EmployeeId = user.Leader1Id.Value,
                    NotificationText = $"Leave deletion notice for {user.FirstName} {user.LastName} - ({user.EmployeeNumber}) | From: {leave.StartDate.ToString("d")} | To: {leave.EndDate.ToString("d")}",
                    NotificationType = leave.Id.ToString(),
                    IsRead = false
                }));
            }
            if (user.Leader2Id != null)
            {
                notifications.Add(SaveNotification(new NotificationDTO
                {
                    EmployeeId = user.Leader2Id.Value,
                    NotificationText = $"Leave deletion notice for {user.FirstName} {user.LastName} ({user.EmployeeNumber}) | From: {leave.StartDate.ToString("d")} | To: {leave.EndDate.ToString("d")}",
                    NotificationType = leave.Id.ToString(),
                    IsRead = false
                }));
            }
            if (user.DefaulLeaderId != null)
            {
                notifications.Add(SaveNotification(new NotificationDTO
                {
                    EmployeeId = user.DefaulLeaderId.Value,
                    NotificationText = $"Leave deletion notice for {user.FirstName} {user.LastName} ({user.EmployeeNumber}) | From: {leave.StartDate.ToString("d")} | To: {leave.EndDate.ToString("d")}",
                    NotificationType = leave.Id.ToString(),
                    IsRead = false
                }));
            }

            await Task.WhenAll(notifications);
        }
        public async Task<ClientResponse> DeleteLeave(Guid id)
        {
            ClientResponse response = new();
            try
            {
                var leave = await _context.EmployeeLeaves
                    .Where(x => x.Id == id && !x.IsDeleted)
                    .FirstOrDefaultAsync();

                if (leave != null)
                {
                    leave.IsDeleted = true;
                    _context.EmployeeLeaves.Update(leave);
                    var res = await _context.SaveChangesAsync();

                    if (res == 0)
                    {
                        response.Message = "Employee Leave Deletion Failed";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        // Fetch the employee information
                        var user = await _context.Employees
                            .Where(x => x.Id == leave.EmployeeId)
                            .Include(x => x.Leader1)
                            .Include(x => x.Leader2)
                            .Include(x => x.DefaulLeader3)
                            .FirstOrDefaultAsync();

                        // Create notifications for the leaders
                        await CreateDeleteNotifications(user, leave);

                        response.Message = "Employee Leave Deleted Successfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Employee Leave does not Exist";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                return response;
            }
        }


    }
}
