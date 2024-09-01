using beONHR.Entities;
using beONHR.Entities.Context;
using System.Net;
using Microsoft.EntityFrameworkCore;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace beONHR.DAL
{


    public interface ILanguageLevelRepo
    {
        Task<ClientResponse> DeleteLanguageLevel(Guid id);
        Task<ClientResponse> GetLanguageLevelById(Guid id);
        Task<ClientResponse> GetLanguageLevel();
        Task<ClientResponse> SaveLanguageLevel(LanguageLevelDTO input);
        Task<ClientResponse> GetFilterLanguageLevel(FilterRequsetDTO filterRequset);
    }
 

    public class LanguageLevelRepo : ILanguageLevelRepo
    {
        private readonly MainContext _context;

        public LanguageLevelRepo(MainContext context)
        {
            _context = context;
        }
        public async Task<ClientResponse> SaveLanguageLevel(LanguageLevelDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var languageLevel = await _context.LanguageLevels.Where(X => X.Level == input.Level && X.IsDeleted != true).FirstOrDefaultAsync();

                    if (languageLevel == null)
                    {
                        LanguageLevel model = new LanguageLevel
                        {
                            Level = input.Level,
                        };
                        await _context.LanguageLevels.AddAsync(model);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "LanguageLevel not saved";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "LanguageLevel saved successfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "LanguageLevel already exists";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = false;
                    }

                    return response;

                }
                else
                {
                    var languageLevel = await _context.LanguageLevels.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefaultAsync();
                    if (languageLevel != null)
                    {
                        languageLevel.Id = input.Id;
                        languageLevel.Level = input.Level;

                        _context.LanguageLevels.Update(languageLevel);
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

        public async Task<ClientResponse> GetLanguageLevel()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var languageLevel = await _context.LanguageLevels.Where(x => x.IsDeleted != true).ToListAsync();

                if (languageLevel == null)
                {
                    response.Message = "No languageLevel found";
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "languageLevel retrieved successfully";
                    response.HttpResponse = languageLevel;
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

        public async Task<ClientResponse> DeleteLanguageLevel(Guid id)
        {
            ClientResponse response = new();
            try
            {

                var languageLevel = await _context.LanguageLevels.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (languageLevel != null)
                {
                    languageLevel.IsDeleted = true;


                    _context.LanguageLevels.Update(languageLevel);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "languageLevel Deleted Faild";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "languageLevel Deleted Sucesfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "languageLevel not Exists";
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

        public async Task<ClientResponse> GetLanguageLevelById(Guid id)
        {
            ClientResponse response = new();
            try
            {

                var languageLevel = await _context.LanguageLevels.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (languageLevel != null)
                {

                    response.Message = "languageLevel Get Sucesfully";
                    response.HttpResponse = languageLevel;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "languageLevel not Exists";
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



        static Expression<Func<LanguageLevel, bool>> CombineLambdas(Expression<Func<LanguageLevel, bool>> expr1, Expression<Func<LanguageLevel, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(LanguageLevel));
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

            return Expression.Lambda<Func<LanguageLevel, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterLanguageLevel(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.LanguageLevels.Where(x => x.IsDeleted != true).AsQueryable();
                // Loop through each filter

                Expression<Func<LanguageLevel, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(LanguageLevel));
                Expression<Func<LanguageLevel, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<LanguageLevel, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<LanguageLevel, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<LanguageLevel, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<LanguageLevel, string>>(SortColumn, parameter);
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
                var LanguageLevel = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseLanguageLevelDTO Response = new ResponseLanguageLevelDTO()
                {
                    languagelevel = LanguageLevel,
                    TotalRecord = totalRecord
                };

                if (LanguageLevel == null)
                {
                    response.Message = "No Any LanguageLevel";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "LanguageLevel Get Sucesfully";
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