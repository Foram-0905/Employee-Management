using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Email;
using beONHR.Entities.DTO.Enum;
using beONHR.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace beONHR.DAL
{
    public interface ILeaveRepo
    {
        Task<ClientResponse> GetLeaveByEmployee(leavesAccordingLogin input);
        Task<ClientResponse> applyLeave(ManageLeaveDTO input);

        Task<ClientResponse> GetAllLeaveByEmployee();
        //Task<ClientResponse> SaveNotifications(NotificationDTO input);
        Task<ClientResponse> ApprovOrRejectLeave(ApprovOrRejectLeave input);
        Task<ClientResponse> GetFilterPendingLeave(leavesAccordingLogin filterRequset);
        Task<ClientResponse> GetFilterLeaveHistory(leavesAccordingLogin filterRequset);
        Task<ClientResponse> GetLeaveByDate(leaveAccordingDate filterRequset);
    }
    public class LeaveRepo : ILeaveRepo
    {
        private readonly MainContext _context;
        private readonly IEmailRepo _emailRepo;
        private readonly DbContextOptions<MainContext> _contextOptions;

        public LeaveRepo(MainContext context, IEmailRepo emailRepo, DbContextOptions<MainContext> contextOptions)
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
                existingNotification.IsRead = input.IsRead =false;
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
                    NotificationText = $"Leave application from {user.FirstName} {user.LastName} ({user.EmployeeNumber}) | From:  {model.StartDate.ToString("d")} | To: {model.EndDate.ToString("d")}",
                    NotificationType = model.Id.ToString(),
                    IsRead = false
                }));
            }
            if (user.DefaulLeaderId != null)
            {
                notifications.Add(SaveNotification(new NotificationDTO
                {
                    EmployeeId = user.DefaulLeaderId.Value,
                    NotificationText = $"Leave application from {user.FirstName} {user.LastName} ({user.EmployeeNumber}) | From:  {model.StartDate.ToString("d")} | To: {model.EndDate.ToString("d")}",
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



        public async Task<ClientResponse> GetLeaveByEmployee(leavesAccordingLogin input)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                List<EmployeeLeave> allLeave = new List<EmployeeLeave>();
                List<EmployeeLeave> pendingLeave = new List<EmployeeLeave>();
                List<EmployeeLeave> leaveHistory = new List<EmployeeLeave>();

                if (input.pageType == "employeeLeaveBalance")
                {
                    allLeave = await _context.EmployeeLeaves.Where(x => x.IsDeleted != true && x.EmployeeId == new Guid(input.Id.ToString())).Include(x => x.LeavetypeId).Include(x => x.Employee).ThenInclude(x => x.Leader1).Include(x => x.Employee).ThenInclude(x => x.Leader2).ToListAsync();
                    pendingLeave = await _context.EmployeeLeaves.Where(x => x.IsDeleted != true && x.EmployeeId == new Guid(input.Id.ToString()) && (x.IsApproved == false && x.IsRejected == false)).Include(x => x.LeavetypeId).Include(x => x.Employee).ThenInclude(x => x.Leader1).Include(x => x.Employee).ThenInclude(x => x.Leader2).ToListAsync();
                    leaveHistory = await _context.EmployeeLeaves.Where(x => x.IsDeleted != true && x.EmployeeId == new Guid(input.Id.ToString()) && (x.IsApproved == true || x.IsRejected == true)).Include(x => x.LeavetypeId).Include(x => x.Employee).ThenInclude(x => x.Leader1).Include(x => x.Employee).ThenInclude(x => x.Leader2).ToListAsync();


                    if (pendingLeave != null || leaveHistory != null)
                    {
                        responseLeave leaves = new responseLeave()
                        {
                            pendingLeaves = pendingLeave,
                            LeavesHistory = leaveHistory,
                            allLeave = allLeave.ToList()
                        };
                        response.Message = "Get Leave";
                        response.HttpResponse = leaves;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        responseLeave leaves = new responseLeave()
                        {
                            pendingLeaves = pendingLeave.ToList(),
                            LeavesHistory = leaveHistory.ToList(),
                            allLeave = allLeave.ToList()
                        };
                        response.Message = "No Any Leave Applied";
                        response.HttpResponse = leaves;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                if (input.pageType == "officeManagementBalance")
                {
                    allLeave = await _context.EmployeeLeaves.Where(x => x.IsDeleted != true && (x.Employee.Leader1Id == new Guid(input.Id) || x.Employee.Leader2Id == new Guid(input.Id) || x.Employee.DefaulLeaderId == new Guid(input.Id))).Include(x => x.LeavetypeId).Include(x => x.Employee).ThenInclude(x => x.Leader1).Include(x => x.Employee).ThenInclude(x => x.Leader2).ToListAsync();
                    pendingLeave = await _context.EmployeeLeaves.Where(x => x.IsDeleted != true && (x.IsApproved == false && x.IsRejected == false) && (x.Employee.Leader1Id == new Guid(input.Id) || x.Employee.Leader2Id == new Guid(input.Id) || x.Employee.DefaulLeaderId == new Guid(input.Id))).Include(x => x.LeavetypeId).Include(x => x.Employee).ThenInclude(x => x.Leader1).Include(x => x.Employee).ThenInclude(x => x.Leader2).ToListAsync();
                    leaveHistory = await _context.EmployeeLeaves.Where(x => x.IsDeleted != true && (x.IsApproved == true || x.IsRejected == true) && (x.Employee.Leader1Id == new Guid(input.Id) || x.Employee.Leader2Id == new Guid(input.Id) || x.Employee.DefaulLeaderId == new Guid(input.Id))).Include(x => x.LeavetypeId).Include(x => x.Employee).ThenInclude(x => x.Leader1).Include(x => x.Employee).ThenInclude(x => x.Leader2).ToListAsync();


                    if (pendingLeave != null || leaveHistory != null)
                    {
                        responseLeave leaves = new responseLeave()
                        {
                            pendingLeaves = pendingLeave.ToList(),
                            LeavesHistory = leaveHistory.ToList(),
                            allLeave = allLeave.ToList()
                        };
                        response.Message = "Get Leave";
                        response.HttpResponse = leaves;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        responseLeave leaves = new responseLeave()
                        {
                            pendingLeaves = pendingLeave.ToList(),
                            LeavesHistory = leaveHistory.ToList(),
                            allLeave = allLeave.ToList()
                        };
                        response.Message = "No Any Leave Applied";
                        response.HttpResponse = leaves;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.HttpResponse = null;
                response.IsSuccess = false;
                response.StatusCode = (HttpStatusCode)ex.HResult;

                return response;
            }
        }

        public async Task<ClientResponse> GetAllLeaveByEmployee()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var allLeaves = await _context.EmployeeLeaves.Where(x => x.IsDeleted != true).Include(x => x.LeavetypeId).Include(x => x.Employee).Include(x => x.Employee).Select(x => new ManageLeaveList
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
                    TeamleadName = x.TeamleadName,
                    OfficeManagementName = x.OfficeManagementName,
                    AppliedDate = x.AppliedDate,
                    ApprovedbyOfficeManagement = x.ApprovedbyOfficeManagement,
                    ApprovedbyTeamlead = x.ApprovedbyTeamlead


                }).ToListAsync();

                if (allLeaves != null)
                {

                    response.Message = "Get Leave";
                    response.HttpResponse = allLeaves.ToList();
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "No Any Leave Applied";
                    response.HttpResponse = null;
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


        // *********************** filter Method start  *********************************//
        static Expression<Func<ManageLeaveList, bool>> CombineLambdas(Expression<Func<ManageLeaveList, bool>> expr1, Expression<Func<ManageLeaveList, bool>> expr2, leavesAccordingLogin filterRequset)
        {
            var parameter = Expression.Parameter(typeof(ManageLeaveList));
            Expression body = null;
            if (filterRequset.filterRequset.filterConditionAndOr == (int)filterConditionAndOrEnum.OrCondition)
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

        public async Task<ClientResponse> GetFilterPendingLeave(leavesAccordingLogin filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                IQueryable<ManageLeaveList> query = Enumerable.Empty<ManageLeaveList>().AsQueryable();
                if (filterRequset.pageType == "employeeLeaveBalance")
                {
                    Regex guidRegex = new Regex(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$");
                   

                    if (guidRegex.IsMatch(filterRequset.Id))
                    {
                        query = _context.EmployeeLeaves.Where(x => x.IsDeleted != true && x.EmployeeId == new Guid(filterRequset.Id.ToString()) && (x.IsApproved == false && x.IsRejected == false)).Include(x => x.LeavetypeId).Include(x => x.Employee).Include(x => x.Employee).Select(x => new ManageLeaveList
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
                            TeamleadName = x.TeamleadName,
                            OfficeManagementName = x.OfficeManagementName,
                            AppliedDate = x.AppliedDate,
                            ApprovedbyOfficeManagement = x.ApprovedbyOfficeManagement,
                            ApprovedbyTeamlead = x.ApprovedbyTeamlead


                        }).AsQueryable();
                    }
                }
                if (filterRequset.pageType == "officeManagementBalance")
                {
                    Regex guidRegex = new Regex(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$");
          

                    if (guidRegex.IsMatch(filterRequset.Id))
                    {
                        var LoginUser = await _context.Employees.Where(x => !x.IsDeleted && x.Id == new Guid(filterRequset.Id)).Include(x => x.Role).FirstOrDefaultAsync();
                        if (LoginUser.Role.NormalizedName != "HR MANAGER")
                        {
                            query = _context.EmployeeLeaves.Where(x => x.IsDeleted != true && x.Employee.IsDeleted == false && (x.IsApproved == false && x.IsRejected == false) && (x.Employee.Leader1Id == new Guid(filterRequset.Id.ToString()) || x.Employee.Leader2Id == new Guid(filterRequset.Id.ToString()) || x.Employee.DefaulLeaderId == new Guid(filterRequset.Id.ToString()))).Include(x => x.LeavetypeId).Include(x => x.Employee).Select(x => new ManageLeaveList
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
                                TeamleadName = x.TeamleadName,
                                OfficeManagementName = x.OfficeManagementName,
                                AppliedDate = x.AppliedDate,
                                ApprovedbyOfficeManagement = x.ApprovedbyOfficeManagement,
                                ApprovedbyTeamlead = x.ApprovedbyTeamlead

                            }).AsQueryable();
                        }
                        else
                        {
                            query = _context.EmployeeLeaves.Where(x => x.IsDeleted != true && x.EmployeeId != new Guid(filterRequset.Id) && x.Employee.IsDeleted != true && (x.IsApproved == false && x.IsRejected == false)).Include(x => x.LeavetypeId).Include(x => x.Employee).Select(x => new ManageLeaveList
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
                                TeamleadName = x.TeamleadName,
                                OfficeManagementName = x.OfficeManagementName,
                                AppliedDate = x.AppliedDate,
                                ApprovedbyOfficeManagement = x.ApprovedbyOfficeManagement,
                                ApprovedbyTeamlead = x.ApprovedbyTeamlead

                            }).AsQueryable();
                        }
                    }
                }

                // Loop through each filter

                Expression<Func<ManageLeaveList, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(ManageLeaveList));
                Expression<Func<ManageLeaveList, bool>> lambda = null;

                if (filterRequset.filterRequset.filterModel != null && filterRequset.filterRequset.filterModel.Count() != 0)
                {
                    foreach (var filter in filterRequset.filterRequset.filterModel)
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
                var totalRecord = query.Count();
                if(totalRecord > 0)
                {
                int skip = (int)((filterRequset.filterRequset.PageNumber - 1) * filterRequset.filterRequset.PageSize);
                    if (filterRequset.filterRequset.sortModel.colId.IndexOf(".") != -1)
                    {
                        // for Make Key and Table subString
                        var SortKey = filterRequset.filterRequset.sortModel.colId.Substring(filterRequset.filterRequset.sortModel.colId.IndexOf(".") + 1).ToString();
                        var ForignKeyTable = filterRequset.filterRequset.sortModel.colId.Substring(0, filterRequset.filterRequset.sortModel.colId.IndexOf(".")).ToString();

                        //make exprssion
                        var ForignKeyTableColumn = Expression.Property(parameter, ForignKeyTable);

                        var sortTableCol = Expression.Property(ForignKeyTableColumn, SortKey);

                        var ForginTableInclude = Expression.Lambda(sortTableCol, parameter);

                        var sortCodition = Expression.Lambda<Func<ManageLeaveList, string>>(sortTableCol, parameter);

                        switch (filterRequset.filterRequset.sortModel.sortOrder)
                        {
                            case "asc":
                                query = query.OrderBy(sortCodition).Skip(skip)
                                .Take((int)(filterRequset.filterRequset.PageSize));

                                break;
                            case "desc":
                                query = query.OrderByDescending(sortCodition).Skip(skip)
                                .Take((int)(filterRequset.filterRequset.PageSize));

                                break;
                        };
                    }
                    else
                    {
                        if (filterRequset.filterRequset.sortModel != null)
                        {

                            var SortColumn = Expression.Property(parameter, filterRequset.filterRequset.sortModel.colId);
                            if (SortColumn.ToString().Contains("Date"))
                            {

                                var sortCodition = Expression.Lambda<Func<ManageLeaveList, DateOnly>>(SortColumn, parameter);

                                switch (filterRequset.filterRequset.sortModel.sortOrder)
                                {
                                    case "asc":
                                        query = query.OrderBy(sortCodition).Skip(skip)
                                    .Take((int)(filterRequset.filterRequset.PageSize));
                                        break;
                                    case "desc":
                                        query = query.OrderByDescending(sortCodition).Skip(skip)
                                    .Take((int)(filterRequset.filterRequset.PageSize));
                                        break;
                                };
                            }
                            else
                            {
                                var sortCodition = Expression.Lambda<Func<ManageLeaveList, string>>(SortColumn, parameter);

                                switch (filterRequset.filterRequset.sortModel.sortOrder)
                                {
                                    case "asc":
                                        query = query.OrderBy(sortCodition).Skip(skip)
                                    .Take((int)(filterRequset.filterRequset.PageSize));
                                        break;
                                    case "desc":
                                        query = query.OrderByDescending(sortCodition).Skip(skip)
                                    .Take((int)(filterRequset.filterRequset.PageSize));
                                        break;
                                };
                            }

                        }
                        else
                        {

                            query = query;
                        }
                    }
                }
               

                //int last =
                var leaves = query.ToList();

                responseManageLeave Response = new responseManageLeave()
                {
                    leaves1 = leaves,
                    TotalRecord = totalRecord
                };

                if (leaves == null)
                {
                    response.Message = "No Any Leave";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Leave Get Sucesfully";
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
        public async Task<ClientResponse> GetFilterLeaveHistory(leavesAccordingLogin filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                IQueryable<ManageLeaveList> query = Enumerable.Empty<ManageLeaveList>().AsQueryable();
                if (filterRequset.pageType == "employeeLeaveBalance")
                {
                    Regex guidRegex = new Regex(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$");


                    if (guidRegex.IsMatch(filterRequset.Id))
                    {
                        query = _context.EmployeeLeaves.Where(x => x.IsDeleted != true && x.EmployeeId == new Guid(filterRequset.Id.ToString()) && (x.IsApproved == true || x.IsRejected == true)).Include(x => x.LeavetypeId).Include(x => x.Employee).Select(x => new ManageLeaveList
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
                            TeamleadName = x.TeamleadName,
                            OfficeManagementName = x.OfficeManagementName,
                            AppliedDate = x.AppliedDate,
                            IsApproved = x.IsApproved,
                            IsRejected = x.IsRejected
                        }).AsQueryable();
                    }
                }
                if (filterRequset.pageType == "officeManagementBalance")
                {
                    Regex guidRegex = new Regex(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$");


                    if (guidRegex.IsMatch(filterRequset.Id))
                    {
                        var LoginUser = await _context.Employees.Where(x => !x.IsDeleted && x.Id == new Guid(filterRequset.Id)).Include(x => x.Role).FirstOrDefaultAsync();
                        if (LoginUser.Role.NormalizedName != "HR MANAGER")
                        {
                            query = _context.EmployeeLeaves.Where(x => x.IsDeleted != true && (x.IsApproved == true || x.IsRejected == true) && (x.Employee.Leader1Id == new Guid(filterRequset.Id.ToString()) || x.Employee.Leader2Id == new Guid(filterRequset.Id.ToString()) || x.Employee.DefaulLeaderId == new Guid(filterRequset.Id.ToString()))).Include(x => x.LeavetypeId).Include(x => x.Employee).Select(x => new ManageLeaveList
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
                                TeamleadName = x.TeamleadName,
                                OfficeManagementName = x.OfficeManagementName,
                                AppliedDate = x.AppliedDate,
                                IsApproved = x.IsApproved,
                                IsRejected = x.IsRejected
                            }).AsQueryable();
                        }
                        else
                        {
                            query = _context.EmployeeLeaves.Where(x => x.IsDeleted != true && (x.IsApproved == true || x.IsRejected == true)).Include(x => x.LeavetypeId).Include(x => x.Employee).Select(x => new ManageLeaveList
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
                                TeamleadName = x.TeamleadName,
                                OfficeManagementName = x.OfficeManagementName,
                                AppliedDate = x.AppliedDate,
                                IsApproved = x.IsApproved,
                                IsRejected = x.IsRejected
                            }).AsQueryable();
                        }
                    }
                }

                // Loop through each filter

                Expression<Func<ManageLeaveList, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(ManageLeaveList));
                Expression<Func<ManageLeaveList, bool>> lambda = null;

                if (filterRequset.filterRequset.filterModel != null && filterRequset.filterRequset.filterModel.Count() != 0)
                {
                    foreach (var filter in filterRequset.filterRequset.filterModel)
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
                                Expression condition = null;
                                if (filter.Value.filterType == "text")
                                {

                                    condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                                    lambda = Expression.Lambda<Func<ManageLeaveList, bool>>(condition, parameter);
                                }
                                if (filter.Value.filterType == "guid")
                                {

                                    condition = Expression.Call(property, typeof(Guid).GetMethod("Equals", new[] { typeof(Guid) }), value);
                                    lambda = Expression.Lambda<Func<ManageLeaveList, bool>>(condition, parameter);
                                }
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
                var totalRecord = query.Count();
                if (totalRecord > 0)
                {
                    int skip = (int)((filterRequset.filterRequset.PageNumber - 1) * filterRequset.filterRequset.PageSize);
                    if (filterRequset.filterRequset.sortModel.colId.IndexOf(".") != -1)
                    {
                        // for Make Key and Table subString
                        var SortKey = filterRequset.filterRequset.sortModel.colId.Substring(filterRequset.filterRequset.sortModel.colId.IndexOf(".") + 1).ToString();
                        var ForignKeyTable = filterRequset.filterRequset.sortModel.colId.Substring(0, filterRequset.filterRequset.sortModel.colId.IndexOf(".")).ToString();

                        //make exprssion
                        var ForignKeyTableColumn = Expression.Property(parameter, ForignKeyTable);

                        var sortTableCol = Expression.Property(ForignKeyTableColumn, SortKey);

                        var ForginTableInclude = Expression.Lambda(sortTableCol, parameter);

                        var sortCodition = Expression.Lambda<Func<ManageLeaveList, string>>(sortTableCol, parameter);

                        switch (filterRequset.filterRequset.sortModel.sortOrder)
                        {
                            case "asc":
                                query = query.OrderBy(sortCodition).Skip(skip)
                                .Take((int)(filterRequset.filterRequset.PageSize));

                                break;
                            case "desc":
                                query = query.OrderByDescending(sortCodition).Skip(skip)
                                .Take((int)(filterRequset.filterRequset.PageSize));

                                break;
                        };
                    }
                    else
                    {
                        if (filterRequset.filterRequset.sortModel != null)
                        {

                            var SortColumn = Expression.Property(parameter, filterRequset.filterRequset.sortModel.colId);
                            if (SortColumn.ToString().Contains("Date"))
                            {

                                var sortCodition = Expression.Lambda<Func<ManageLeaveList, DateOnly>>(SortColumn, parameter);

                                switch (filterRequset.filterRequset.sortModel.sortOrder)
                                {
                                    case "asc":
                                        query = query.OrderBy(sortCodition).Skip(skip)
                                    .Take((int)(filterRequset.filterRequset.PageSize));
                                        break;
                                    case "desc":
                                        query = query.OrderByDescending(sortCodition).Skip(skip)
                                    .Take((int)(filterRequset.filterRequset.PageSize));
                                        break;
                                };
                            }
                            else
                            {
                                var sortCodition = Expression.Lambda<Func<ManageLeaveList, string>>(SortColumn, parameter);

                                switch (filterRequset.filterRequset.sortModel.sortOrder)
                                {
                                    case "asc":
                                        query = query.OrderBy(sortCodition).Skip(skip)
                                    .Take((int)(filterRequset.filterRequset.PageSize));
                                        break;
                                    case "desc":
                                        query = query.OrderByDescending(sortCodition).Skip(skip)
                                    .Take((int)(filterRequset.filterRequset.PageSize));
                                        break;
                                };
                            }

                        }
                        else
                        {

                            query = query;
                        }
                    }
                }

                //int last =
                var leaves =  query.ToList();


                responseManageLeave Response = new responseManageLeave()
                {
                    leaves1 = leaves,
                    TotalRecord = totalRecord
                };

                response.Message = "Desigantion Get Sucesfully";
                response.HttpResponse = Response;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                //if (leaves.Count() == 0)
                //{
                //    response.Message = "No Any Desigantion";
                //    response.HttpResponse = null;
                //    response.IsSuccess = true;
                //    response.StatusCode = HttpStatusCode.OK;
                //}
                //else
                //{
                //    response.Message = "Desigantion Get Sucesfully";
                //    response.HttpResponse = Response;
                //    response.IsSuccess = true;
                //    response.StatusCode = HttpStatusCode.OK;
                //}


                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }


        // *********************** filter Method End  *********************************//
        public async Task<ClientResponse> ApprovOrRejectLeave(ApprovOrRejectLeave input)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var leaveAction = "";
                var leave = await _context.EmployeeLeaves.Where(x => x.IsDeleted == false && x.Id == input.Id).Include(x => x.Employee).FirstOrDefaultAsync();

                //CHECK EMAPLOYEE HAVE LEAVE BALANCE

                if (leave != null)
                {
                    var leaderName = _context.Employees.Where(x => x.Id == new Guid(input.approvedByName)).FirstOrDefaultAsync();
                    if (input.approvedOrreject == "Approve")
                    {
                        if (input.approvedBy == "TeamLead")
                        {
                            leave.ApprovedbyTeamlead = true;
                            leave.TeamleadName = leaderName.Result.FirstName + " " + leaderName.Result.MiddleName + " " + leaderName.Result.LastName;
                            leaveAction = "Approved";
                        }
                        if (input.approvedBy == "OfficeMangement")
                        {

                            leave.ApprovedbyOfficeManagement = true;
                            leave.IsApproved = true;
                            leave.OfficeManagementName = leaderName.Result.FirstName + " " + leaderName.Result.MiddleName + " " + leaderName.Result.LastName;
                            leaveAction = "Approved";

                            //UPDATE EAPLOYEE LEAVE BALANCE

                        }
                        _context.EmployeeLeaves.Update(leave);
                        var res = await _context.SaveChangesAsync();
                        if (res == 0)
                        {
                            response.Message = "Leave Approved Faild ";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Leave Approved Sucesfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }

                    }
                    if (input.approvedOrreject == "Rejecte")
                    {
                        if (input.approvedBy == "TeamLead")
                        {

                            leave.IsRejected = true;
                            leave.TeamleadName = leaderName.Result.FirstName + " " + leaderName.Result.MiddleName + " " + leaderName.Result.LastName;
                            leaveAction = "Rejected";

                            //add reson
                        }
                        if (input.approvedBy == "OfficeMangement")
                        {

                            leave.IsRejected = true;
                            leave.OfficeManagementName = leaderName.Result.FirstName + " " + leaderName.Result.MiddleName + " " + leaderName.Result.LastName;
                            leaveAction = "Rejected";

                        }
                        _context.EmployeeLeaves.Update(leave);
                        var res = await _context.SaveChangesAsync();
                        if (res == 0)
                        {
                            response.Message = "Leave Reject Faild ";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Leave Rejected Sucesfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    if (response.IsSuccess)
                    {
                        EmailMessage options = new EmailMessage
                        {
                            ToEmails = new List<string>() { leave.Employee.Email },

                            PlaceHolders = new List<KeyValuePair<string, string>>()
                            {
                               new KeyValuePair<string, string>("{{leaderName}}", leaderName.Result.FirstName + " " + leaderName.Result.MiddleName + " " + leaderName.Result.LastName),
                               new KeyValuePair<string, string>("{{EmployeeName}}", leave.Employee.FirstName + " " + leave.Employee.MiddleName + " " + leave.Employee.LastName),
                               new KeyValuePair<string, string>("{{StartDate}}",leave.StartDate.ToString()),
                               new KeyValuePair<string, string>("{{EndDate}}",leave.EndDate.ToString()),
                               new KeyValuePair<string, string>("{{Action}}",leaveAction),

                            }
                        };
                        await SaveNotification(new NotificationDTO
                        {
                            EmployeeId = leave.EmployeeId,
                            NotificationText = $"Your leave application has been {leaveAction} by {leave.TeamleadName} | From: {leave.StartDate.ToString("d")} | To: {leave.EndDate.ToString("d")}",
                            NotificationType = leave.Id.ToString(),
                            IsRead = false
                        });

                        await _emailRepo.SendEmailActionOnLeave(options);
                    }
                }

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Notification for user
        //public async Task<ClientResponse> SaveNotifications(NotificationDTO input)
        //{
        //    ClientResponse response = new ClientResponse();

        //    using (var context = new MainContext(_contextOptions))
        //    {
        //        try
        //        {
        //            var notification = new Notification
        //            {
        //                NotificationID = Guid.NewGuid(),
        //                EmployeeId = input.EmployeeId,
        //                NotificationText = input.NotificationText,
        //                NotificationType = input.NotificationType,
        //                IsRead = input.IsRead,
        //                CreatedAt = DateTime.Now
        //            };

        //            await context.Notifications.AddAsync(notification);
        //            var result = await context.SaveChangesAsync();

        //            if (result == 0)
        //            {
        //                response.Message = "Notification not saved";
        //                response.StatusCode = HttpStatusCode.NoContent;
        //                response.IsSuccess = false;
        //            }
        //            else
        //            {
        //                response.Message = "Notification saved successfully";
        //                response.HttpResponse = notification.NotificationID;
        //                response.IsSuccess = true;
        //                response.StatusCode = HttpStatusCode.OK;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            response.Message = ex.Message;
        //            response.StatusCode = HttpStatusCode.InternalServerError;
        //            response.IsSuccess = false;
        //        }

        //        return response;
        //    }
        //}


        static Expression<Func<ManageLeaveList, bool>> CombineLambdasDate(Expression<Func<ManageLeaveList, bool>> expr1, Expression<Func<ManageLeaveList, bool>> expr2, leaveAccordingDate filterRequset)
        {
            var parameter = Expression.Parameter(typeof(ManageLeaveList));
            Expression body = null;
            if (filterRequset.filterRequset.filterConditionAndOr == (int)filterConditionAndOrEnum.OrCondition)
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

        public async Task<ClientResponse> GetLeaveByDate(leaveAccordingDate filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                IQueryable<ManageLeaveList> query = Enumerable.Empty<ManageLeaveList>().AsQueryable();
                //query =  _context.EmployeeLeaves.Include(x => x.LeavetypeId).Include(x => x.Employee).AsEnumerable().Where(x => x.IsDeleted != true && (DateTime.Parse(x.StartDate.ToString()) <= DateTime.Parse(filterRequset.Date) && DateTime.Parse(x.EndDate.ToString()) >= DateTime.Parse(filterRequset.Date))).Select(x => new ManageLeaveList
                //{
                //    Id = x.Id,
                //    StartDate = x.StartDate,
                //    EndDate = x.EndDate,
                //    Leavetype = x.Leavetype,
                //    LeavetypeName = x.LeavetypeId.TypeName,
                //    EmployeeId = x.EmployeeId,
                //    Leader1 = x.Employee.Leader1Id,
                //    Leader2 = x.Employee.Leader2Id,
                //    Leader3 = x.Employee.DefaulLeaderId,
                //    LeaveDay = x.LeaveDay,
                //    leave_duration = x.leave_duration,
                //    Leave_Start_From = x.Leave_Start_From,
                //    Leave_End = x.Leave_End,
                //    Reason = x.Reason,
                //    AppliedDate=x.AppliedDate,
                //    EmployeeName = x.Employee.FirstName + " " + x.Employee.MiddleName + " " + x.Employee.LastName,
                //}).AsQueryable();

                Regex guidRegex = new Regex(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$");


                if (guidRegex.IsMatch(filterRequset.Id))
                {
                    var LoginUser = await _context.Employees.Where(x => !x.IsDeleted && x.Id == new Guid(filterRequset.Id)).Include(x => x.Role).FirstOrDefaultAsync();
                    if (LoginUser.Role.NormalizedName != "HR MANAGER")
                    {
                        query = _context.EmployeeLeaves.Where(x => x.IsDeleted != true && x.Employee.IsDeleted == false && (x.Employee.Leader1Id == new Guid(filterRequset.Id.ToString()) || x.Employee.Leader2Id == new Guid(filterRequset.Id.ToString()) || x.Employee.DefaulLeaderId == new Guid(filterRequset.Id.ToString())) && (x.StartDate <= DateOnly.FromDateTime(DateTime.Parse(filterRequset.Date)) && x.EndDate >= DateOnly.FromDateTime(DateTime.Parse(filterRequset.Date)))).Include(x => x.LeavetypeId).Include(x => x.Employee).Select(x => new ManageLeaveList
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
                            TeamleadName = x.TeamleadName,
                            OfficeManagementName = x.OfficeManagementName,
                            AppliedDate = x.AppliedDate,
                            ApprovedbyOfficeManagement = x.ApprovedbyOfficeManagement,
                            ApprovedbyTeamlead = x.ApprovedbyTeamlead

                        }).AsQueryable();
                    }
                    else
                    {
                        query = _context.EmployeeLeaves.Where(x => x.IsDeleted != true && x.EmployeeId != new Guid(filterRequset.Id) && x.Employee.IsDeleted != true && (x.StartDate <= DateOnly.FromDateTime(DateTime.Parse(filterRequset.Date)) && x.EndDate >= DateOnly.FromDateTime(DateTime.Parse(filterRequset.Date)))).Include(x => x.LeavetypeId).Include(x => x.Employee).Select(x => new ManageLeaveList
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
                            TeamleadName = x.TeamleadName,
                            OfficeManagementName = x.OfficeManagementName,
                            AppliedDate = x.AppliedDate,
                            ApprovedbyOfficeManagement = x.ApprovedbyOfficeManagement,
                            ApprovedbyTeamlead = x.ApprovedbyTeamlead

                        }).AsQueryable();
                    }
                }
                Expression<Func<ManageLeaveList, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(ManageLeaveList));
                Expression<Func<ManageLeaveList, bool>> lambda = null;

                if (filterRequset.filterRequset.filterModel != null && filterRequset.filterRequset.filterModel.Count() != 0)
                {
                    foreach (var filter in filterRequset.filterRequset.filterModel)
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
                                combinedCondition = CombineLambdasDate(combinedCondition, lambda, filterRequset);

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
                                combinedCondition = CombineLambdasDate(combinedCondition, lambda, filterRequset);


                            }
                        }
                    }

                    query = query.Where(combinedCondition);


                }
                var totalRecord = query.Count();
                int skip = (int)((filterRequset.filterRequset.PageNumber - 1) * filterRequset.filterRequset.PageSize);
                if (filterRequset.filterRequset.sortModel.colId.IndexOf(".") != -1)
                {
                    // for Make Key and Table subString
                    var SortKey = filterRequset.filterRequset.sortModel.colId.Substring(filterRequset.filterRequset.sortModel.colId.IndexOf(".") + 1).ToString();
                    var ForignKeyTable = filterRequset.filterRequset.sortModel.colId.Substring(0, filterRequset.filterRequset.sortModel.colId.IndexOf(".")).ToString();

                    //make exprssion
                    var ForignKeyTableColumn = Expression.Property(parameter, ForignKeyTable);

                    var sortTableCol = Expression.Property(ForignKeyTableColumn, SortKey);

                    var ForginTableInclude = Expression.Lambda(sortTableCol, parameter);

                    var sortCodition = Expression.Lambda<Func<ManageLeaveList, string>>(sortTableCol, parameter);

                    switch (filterRequset.filterRequset.sortModel.sortOrder)
                    {
                        case "asc":
                            query = query.OrderBy(sortCodition).Skip(skip)
                            .Take((int)(filterRequset.filterRequset.PageSize));

                            break;
                        case "desc":
                            query = query.OrderByDescending(sortCodition).Skip(skip)
                            .Take((int)(filterRequset.filterRequset.PageSize));

                            break;
                    };
                }
                else
                {
                    if (filterRequset.filterRequset.sortModel != null)
                    {

                        var SortColumn = Expression.Property(parameter, filterRequset.filterRequset.sortModel.colId);
                        if (SortColumn.ToString().Contains("Date"))
                        {

                            var sortCodition = Expression.Lambda<Func<ManageLeaveList, DateOnly>>(SortColumn, parameter);

                            switch (filterRequset.filterRequset.sortModel.sortOrder)
                            {
                                case "asc":
                                    query = query.OrderBy(sortCodition).Skip(skip)
                                .Take((int)(filterRequset.filterRequset.PageSize));
                                    break;
                                case "desc":
                                    query = query.OrderByDescending(sortCodition).Skip(skip)
                                .Take((int)(filterRequset.filterRequset.PageSize));
                                    break;
                            };
                        }
                        else
                        {
                            var sortCodition = Expression.Lambda<Func<ManageLeaveList, string>>(SortColumn, parameter);

                            switch (filterRequset.filterRequset.sortModel.sortOrder)
                            {
                                case "asc":
                                    query = query.OrderBy(sortCodition).Skip(skip)
                                .Take((int)(filterRequset.filterRequset.PageSize));
                                    break;
                                case "desc":
                                    query = query.OrderByDescending(sortCodition).Skip(skip)
                                .Take((int)(filterRequset.filterRequset.PageSize));
                                    break;
                            };
                        }

                    }
                    else
                    {
                        query = query;
                    }
                }

                //int last =
                var leaves = query.ToList();

                responseManageLeave Response = new responseManageLeave()
                {
                    leaves1 = leaves,
                    TotalRecord = totalRecord
                };

                if (leaves == null)
                {
                    response.Message = "No Any Leave";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Leave Get Sucesfully";
                    response.HttpResponse = Response;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                //if (leave != null || leave != null)
                //{

                //    response.Message = "Get Leave";
                //    response.HttpResponse = leave;
                //    response.IsSuccess = true;
                //    response.StatusCode = HttpStatusCode.OK;
                //}
                //else
                //{
                //    response.Message = "No Any Leave Applied";
                //    response.HttpResponse = null;
                //    response.IsSuccess = true;
                //    response.StatusCode = HttpStatusCode.OK;
                //}
                return response;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
