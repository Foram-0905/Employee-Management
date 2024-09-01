using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface ILeaveTypeRepo
    {
        Task<ClientResponse> SaveLeaveType(LeaveTypeDTO input);
        Task<ClientResponse> GetLeaveType();
        Task<ClientResponse> DeleteLeaveType(Guid id);
        Task<ClientResponse> GetLeaveTypeById(Guid id);
        Task<ClientResponse> GetFilterLeaveType(FilterRequsetDTO filterRequset);

    }

    public class LeaveTypeRepo : ILeaveTypeRepo
    {
        private readonly MainContext _context;
        public LeaveTypeRepo(MainContext context)
        {
            _context = context;
        }
        public async Task<ClientResponse> SaveLeaveType(LeaveTypeDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var existingLeaveType = await _context.LeaveTypes
                        .Where(x => x.TypeName == input.TypeName && x.CategoryName == input.CategoryName && x.IsDeleted != true)
                        .FirstOrDefaultAsync();

                    if (existingLeaveType == null)
                    {
                        LeaveType model = new LeaveType
                        {
                            TypeName = input.TypeName,
                            CategoryName = input.CategoryName
                        };

                        await _context.LeaveTypes.AddAsync(model);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Leave type not inserted";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Leave type inserted successfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Leave type already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }
                    return response;
                }
                else
                {
                    var leaveType = await _context.LeaveTypes
                        .Where(x => x.Id == input.Id && x.IsDeleted == false)
                        .FirstOrDefaultAsync();

                    if (leaveType != null)
                    {
                        leaveType.Id = input.Id;
                        leaveType.TypeName = input.TypeName;
                        leaveType.CategoryName = input.CategoryName;

                        _context.LeaveTypes.Update(leaveType);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Leave type not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Leave type updated successfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Leave type does not exist";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetLeaveType()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var leaveTypes = await _context.LeaveTypes
                    .Where(x => x.IsDeleted != true)
                    .Include(x => x.LeaveCategory)
                    .ToListAsync();

                if (leaveTypes == null || leaveTypes.Count == 0)
                {
                    response.Message = "No leave types found";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Leave types retrieved successfully";
                    response.HttpResponse = leaveTypes;
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
        // *********************** Filter Method Start   *********************************//



        static Expression<Func<LeaveType, bool>> CombineLambdas(Expression<Func<LeaveType, bool>> expr1, Expression<Func<LeaveType, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(LeaveType));
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

            return Expression.Lambda<Func<LeaveType, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterLeaveType(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.LeaveTypes.Where(x => x.IsDeleted != true).Include(x => x.LeaveCategory).AsQueryable();
                // Loop through each filter

                Expression<Func<LeaveType, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(LeaveType));
                Expression<Func<LeaveType, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<LeaveType, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<LeaveType, bool>>(condition, parameter);
                            //query = query.Where(lambda);
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

                    var sortCodition = Expression.Lambda<Func<LeaveType, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<LeaveType, string>>(SortColumn, parameter);
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
                var totalRecord = query.Count();
                int skip = (int)((filterRequset.PageNumber - 1) * filterRequset.PageSize);
                //int last =
                var leaveType = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseLeaveTypeDto Response = new ResponseLeaveTypeDto()
                {
                    leaveType = leaveType,
                    TotalRecord = totalRecord
                };

                if (leaveType == null)
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
        public async Task<ClientResponse> DeleteLeaveType(Guid id)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var leaveType = await _context.LeaveTypes
                    .Where(x => x.Id == id && x.IsDeleted != true)
                    .FirstOrDefaultAsync();

                if (leaveType != null)
                {
                    leaveType.IsDeleted = true;
                    _context.LeaveTypes.Update(leaveType);
                    var res = await _context.SaveChangesAsync();

                    if (res == 0)
                    {
                        response.Message = "Failed to delete leave type";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Leave type deleted successfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Leave type does not exist";
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

        public async Task<ClientResponse> GetLeaveTypeById(Guid id)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var leaveType = await _context.LeaveTypes
                    .Where(x => x.Id == id && x.IsDeleted != true)
                    .FirstOrDefaultAsync();

                if (leaveType != null)
                {
                    response.Message = "Leave type retrieved successfully";
                    response.HttpResponse = leaveType;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Leave type does not exist";
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
    }
}
