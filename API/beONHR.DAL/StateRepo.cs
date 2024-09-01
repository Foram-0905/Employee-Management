using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beONHR.Entities.Context;
using Microsoft.EntityFrameworkCore;
using beONHR.Entities.DTO.Enum;
using beONHR.Entities.DTO;
using beONHR.Entities;
using System.Net;
using System.Linq.Expressions;

namespace beONHR.DAL
{
    public interface IStateRepo
    {
        Task<ClientResponse> SaveState(StateDto input);
        Task<ClientResponse> GetState();
        Task<ClientResponse> DeleteState(Guid id);
        Task<ClientResponse> GetStateById(Guid id);
        Task<ClientResponse> GetStatesByCountryId(Guid countryId);
        Task<ClientResponse> GetFilterState(FilterRequsetDTO filterRequset);
    }

    public class StateRepo : IStateRepo
    {
        private readonly MainContext _context;

        public StateRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveState(StateDto input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var state = await _context.States.Where(x => x.Name == input.Name && x.CountryId == input.CountryId && x.IsDeleted != true).FirstOrDefaultAsync();
                    if (state == null)
                    {
                        State model = new State
                        {
                            Name = input.Name,
                            CountryId = input.CountryId
                        };

                        await _context.States.AddAsync(model);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "State not insert";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "State insert Sucesfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "State already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }
                    return response;

                }
                else
                {
                    var state = await _context.States.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefaultAsync();
                    if (state != null)
                    {


                        state.Id = input.Id;
                        state.Name = input.Name;
                        state.CountryId = input.CountryId;

                        _context.States.Update(state);
                        var res = await _context.SaveChangesAsync();
                        if (res == 0)
                        {
                            response.Message = "state not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "state updated Sucesfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }

                    }
                    else
                    {
                        response.Message = "state Not Found";
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

        public async Task<ClientResponse> GetState()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var state = await _context.States.Include(x => x.country).Where(x => x.IsDeleted != true).OrderBy( x => x.Name).ToListAsync();

                if (state == null)
                {
                    response.Message = "No Any State";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                response.Message = "State Get Sucesfully";
                response.HttpResponse = state;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ClientResponse> DeleteState(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var state = await _context.States.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (state != null)
                {
                    state.IsDeleted = true;


                    _context.States.Update(state);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "state Deleted Faild";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "state Deleted Sucesfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "state not Exists";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = true;
                }

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ClientResponse> GetStateById(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var state = await _context.States.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (state != null)
                {

                    response.Message = "States Get Sucesfully";
                    response.HttpResponse = state;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "States not Exists";
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

        public async Task<ClientResponse> GetStatesByCountryId(Guid countryId)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var states = await _context.States
                                           .Where(x => x.CountryId == countryId && x.IsDeleted != true)
                                           .OrderBy(x => x.Name)
                                           .ToListAsync();

                if (states == null || states.Count == 0)
                {
                    response.Message = "No states found for the given CountryId";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "States retrieved successfully";
                    response.HttpResponse = states;
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

        // *********************** Filter Method Start   *********************************//

        static Expression<Func<State, bool>> CombineLambdas(Expression<Func<State, bool>> expr1, Expression<Func<State, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(State));
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

            return Expression.Lambda<Func<State, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterState(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.States.Where(x => x.IsDeleted != true).Include(x => x.country).AsQueryable();
                // Loop through each filter

                Expression<Func<State, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(State));
                Expression<Func<State, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<State, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<State, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<State, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<State, string>>(SortColumn, parameter);
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
                var state = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseStateDto Response = new ResponseStateDto()
                {
                    state = state,
                    TotalRecord = totalRecord
                };

                if (state == null)
                {
                    response.Message = "No Any state";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "state Get Sucesfully";
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
