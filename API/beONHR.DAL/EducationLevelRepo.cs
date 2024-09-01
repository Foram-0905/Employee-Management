using beONHR.Entities.Context;
using beONHR.Entities;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace beONHR.DAL
{
    public interface IEductionLevelRepo
    {
        Task<ClientResponse> DeleteEductionLevel(Guid id);
        Task<ClientResponse> GetEductionLevelById(Guid id);
        Task<ClientResponse> GetEductionLevel();
        Task<ClientResponse> SaveEductionLevel(EductionLevelDTO input);
        Task<ClientResponse> GetFilterEductionLevel(FilterRequsetDTO filterRequset);
    }
    public class EductionLevelRepo : IEductionLevelRepo
    {
        private readonly MainContext _context;

        public EductionLevelRepo(MainContext context)
        {
            _context = context;
        }
        public async Task<ClientResponse> SaveEductionLevel(EductionLevelDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var eductionLevel = await _context.EductionLevels.Where(x => x.Level == input.Level && x.IsDeleted != true).FirstOrDefaultAsync();

                    if (eductionLevel == null)
                    {
                        EductionLevel model = new EductionLevel
                        {
                            Level = input.Level,
                        };
                        await _context.EductionLevels.AddAsync(model);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Eduction Levels not saved";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Eduction Levels saved successfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Eduction Levels already exists";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = false;
                    }

                    return response;

                }
                else
                {
                    var eductionLevel = await _context.EductionLevels.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefaultAsync();
                    if (eductionLevel != null)
                    {
                        //eductionLevel.Id = input.Id;
                        eductionLevel.Level = input.Level;

                        _context.EductionLevels.Update(eductionLevel);
                        var res = await _context.SaveChangesAsync();
                        if (res == 0)
                        {
                            response.Message = "Eduction Levels not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Eduction Levels updated Sucesfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Eduction Levels Not Found";
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

        public async Task<ClientResponse> GetEductionLevel()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var eductionLevel = await _context.EductionLevels.Where(x => x.IsDeleted != true).OrderBy(x => x.Level).ToListAsync();

                if (eductionLevel == null)
                {
                    response.Message = "No Eduction Levels found";
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Eduction Levels retrieved successfully";
                    response.HttpResponse = eductionLevel;
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

        public async Task<ClientResponse> DeleteEductionLevel(Guid id)
        {
            ClientResponse response = new();
            try
            {

                var eductionLevel = await _context.EductionLevels.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (eductionLevel != null)
                {
                    eductionLevel.IsDeleted = true;


                    _context.EductionLevels.Update(eductionLevel);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "Eduction Levels Deleted Faild";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Eduction Levels Deleted Sucesfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Eduction Levels not Exists";
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

        public async Task<ClientResponse> GetEductionLevelById(Guid id)
        {
            ClientResponse response = new();
            try
            {

                var eductionLevel = await _context.EductionLevels.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (eductionLevel != null)
                {

                    response.Message = "Eduction Levels Get Sucesfully";
                    response.HttpResponse = eductionLevel;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "Eduction Levels not Exists";
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



        static Expression<Func<EductionLevel, bool>> CombineLambdas(Expression<Func<EductionLevel, bool>> expr1, Expression<Func<EductionLevel, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(EductionLevel));
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

            return Expression.Lambda<Func<EductionLevel, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterEductionLevel(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.EductionLevels.Where(x => x.IsDeleted != true).AsQueryable();
                // Loop through each filter

                Expression<Func<EductionLevel, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(EductionLevel));
                Expression<Func<EductionLevel, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<EductionLevel, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<EductionLevel, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<EductionLevel, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<EductionLevel, string>>(SortColumn, parameter);
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
                var eductionlevel = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseEductionLevelDTO Response = new ResponseEductionLevelDTO()
                {
                    eductionlevel = eductionlevel,
                    TotalRecord = totalRecord
                };

                if (eductionlevel == null)
                {
                    response.Message = "No Any eductionlevel";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "eductionlevel Get Sucesfully";
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



    }
}