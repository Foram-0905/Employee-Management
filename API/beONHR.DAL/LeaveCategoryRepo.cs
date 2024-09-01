
using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface ILeaveCategoryRepo
    {
        Task<ClientResponse> SaveLeaveCategory(LeaveCategoryDTO input);
        Task<ClientResponse> GetLeaveCategory();
        Task<ClientResponse> DeleteLeaveCategory(Guid leaveCategoryId);
        Task<ClientResponse> GetLeaveCategoryById(Guid id);
        Task<ClientResponse> GetFilterLeaveCategory(FilterRequsetDTO filterRequset);

    }

    public class LeaveCategoryRepo : ILeaveCategoryRepo
    {
        private readonly MainContext _context;

        public LeaveCategoryRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveLeaveCategory(LeaveCategoryDTO input)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var existingCategory = await _context.LeaveCategories.FirstOrDefaultAsync(x => x.Name == input.Name && x.IsDeleted != true);

                    if (existingCategory == null)
                    {
                        var category = new LeaveCategory
                        {
                            Id = Guid.NewGuid(),
                            Name = input.Name
                            // Add other properties as needed
                        };

                        _context.LeaveCategories.Add(category);
                        await _context.SaveChangesAsync();

                        response.Message = "Leave category added successfully";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Message = "Leave category with the same name already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    var category = await _context.LeaveCategories.FindAsync(input.Id);

                    if (category != null)
                    {
                        category.Name = input.Name;
                        // Update other properties as needed

                        await _context.SaveChangesAsync();

                        response.Message = "Leave category updated successfully";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Message = "Leave category not found";
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.IsSuccess = false;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetLeaveCategory()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var categories = await _context.LeaveCategories.Where(x => x.IsDeleted != true).ToListAsync();

                if (categories != null && categories.Count > 0)
                {
                    response.Message = "Leave categories retrieved successfully";
                    response.HttpResponse = categories;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "No leave categories found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // *********************** Filter Method Start   *********************************//



        static Expression<Func<LeaveCategory, bool>> CombineLambdas(Expression<Func<LeaveCategory, bool>> expr1, Expression<Func<LeaveCategory, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(LeaveCategory));
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

            return Expression.Lambda<Func<LeaveCategory, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterLeaveCategory(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.LeaveCategories.Where(x => x.IsDeleted != true).AsQueryable();
                // Loop through each filter

                Expression<Func<LeaveCategory, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(LeaveCategory));
                Expression<Func<LeaveCategory, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<LeaveCategory, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<LeaveCategory, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<LeaveCategory, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<LeaveCategory, string>>(SortColumn, parameter);
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
                var leaveCategory = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseLeaveCategoryDto Response = new ResponseLeaveCategoryDto()
                {
                    leaveCategory = leaveCategory,
                    TotalRecord = totalRecord
                };

                if (leaveCategory == null)
                {
                    response.Message = "No Any leaveCategory";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "leaveCategory Get Sucesfully";
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
        public async Task<ClientResponse> DeleteLeaveCategory(Guid id)
        {
            ClientResponse response = new();
            try
            {

                var leavecategory = await _context.LeaveCategories.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (leavecategory != null)
                {
                    leavecategory.IsDeleted = true;


                    _context.LeaveCategories.Update(leavecategory);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "Leavecategory Deleted Failed";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Leavecategory Deleted Successfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Leavecategory not Exists";
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

        public async Task<ClientResponse> GetLeaveCategoryById(Guid id)
        {
            ClientResponse response = new();
            try
            {
                var category = await _context.LeaveCategories.FindAsync(id);

                if (category != null)
                {
                    response.Message = "Leave category retrieved successfully";
                    response.HttpResponse = category;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "Leave category not found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
