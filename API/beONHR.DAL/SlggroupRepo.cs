using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beONHR.Entities;
using beONHR.Entities.Context;
using System.Net;
using Microsoft.EntityFrameworkCore;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using System.Runtime.InteropServices;
using System.Linq.Expressions;

namespace beONHR.DAL
{
    public interface ISlggroupRepo
    {
        Task<ClientResponse> DeleteSlggroup(Guid id);

        Task<ClientResponse> GetSLGgroupById(Guid id);
        Task<ClientResponse> GetSlggroup();
        Task<ClientResponse> SaveSlggroup(SlggroupDto input);
        Task<ClientResponse> GetFilterSlggroup(FilterRequsetDTO filterRequset);
    }
    public class SlggroupRepo : ISlggroupRepo
    {
        private readonly MainContext _context;

        public SlggroupRepo(MainContext context)
        {
            _context = context;
        }
        public async Task<ClientResponse> SaveSlggroup(SlggroupDto input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var slggroup = await _context.SLGGroups.Where(x => x.StatusName == input.StatusName && x.InitialStatus == input.InitialStatus && x.StatusSequence == input.StatusSequence && x.RelevantExperience == input.RelevantExperience && x.IsDeleted != true).FirstOrDefaultAsync();

                    if (slggroup == null)
                    {
                        SLGGroup model = new SLGGroup
                        {

                            StatusName = input.StatusName,
                            InitialStatus = input.InitialStatus,
                            StatusSequence = input.StatusSequence,
                            RelevantExperience = input.RelevantExperience
                        };

                        await _context.SLGGroups.AddAsync(model);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Slggroup not saved";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Slggroup saved successfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Slggroup already exists";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = false;
                    }

                    return response;
                }
                else
                {
                    var slggroup = await _context.SLGGroups.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefaultAsync();
                    if (slggroup != null)
                    {
                        slggroup.Id = input.Id;
                        slggroup.StatusName = input.StatusName;
                        slggroup.InitialStatus = input.InitialStatus;
                        slggroup.StatusSequence = input.StatusSequence;
                        slggroup.RelevantExperience = input.RelevantExperience;

                        _context.SLGGroups.Update(slggroup);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "slggroup not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "slggroup updated Sucesfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "slggroup Not Found";
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

        public async Task<ClientResponse> GetSlggroup()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var SLGGroup = await _context.SLGGroups.Where(x => x.IsDeleted != true).OrderByDescending(x=>x.StatusName).ToListAsync();

                if (SLGGroup == null)
                {
                    response.Message = "No slggroup found";
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "slggroup retrieved successfully";
                    response.HttpResponse = SLGGroup;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteSlggroup(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var slggroup = await _context.SLGGroups.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (slggroup != null)
                {
                    slggroup.IsDeleted = true;


                    _context.SLGGroups.Update(slggroup);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "slggroup Deleted Faild";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "slggroup Deleted Sucesfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "slggroup not Exists";
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

        public async Task<ClientResponse> GetSLGgroupById(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var slgGroup = await _context.SLGGroups.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (slgGroup != null)
                {

                    response.Message = "SLGGroup Get Sucesfully";
                    response.HttpResponse = slgGroup;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "SLGGroup not Exists";
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
        // *********************** Filter Method Start   *********************************//



        static Expression<Func<SLGGroup, bool>> CombineLambdas(Expression<Func<SLGGroup, bool>> expr1, Expression<Func<SLGGroup, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(SLGGroup));
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

            return Expression.Lambda<Func<SLGGroup, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterSlggroup(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.SLGGroups.Where(x => x.IsDeleted != true).AsQueryable();
                // Loop through each filter

                Expression<Func<SLGGroup, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(SLGGroup));
                Expression<Func<SLGGroup, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<SLGGroup, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<SLGGroup, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<SLGGroup, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<SLGGroup, string>>(SortColumn, parameter);
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
                var slggroups = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseSlggroupDto Response = new ResponseSlggroupDto()
                {
                    slggroups = slggroups,
                    TotalRecord = totalRecord
                };

                if (slggroups == null)
                {
                    response.Message = "No Any slggroups";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "slggroups Get Sucesfully";
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
    }

}



