using beONHR.Entities.DTO.ForgotPassword;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beONHR.Entities;
using beONHR.Entities.Context;
using System.Diagnostics;
using System.Net;
using Microsoft.EntityFrameworkCore;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Crypto;
using Azure;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace beONHR.DAL
{
    public interface ICityRepo
    {
        Task<ClientResponse> SaveCity(CityDto input);
        Task<ClientResponse> GetFilterCity(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetCities();
        Task<ClientResponse> GetCityByState(Guid stateId);
        Task<ClientResponse> DeleteCity(Guid cityId);
        Task<ClientResponse> GetCityById(Guid id);
    }

    public class CityRepo : ICityRepo
    {
        private readonly MainContext _context;

        public CityRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveCity(CityDto input)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var existingCity = await _context.Cities.FirstOrDefaultAsync(x => x.Name == input.Name && x.IsDeleted != true);

                    if (existingCity == null)
                    {
                        var city = new City
                        {
                            Id = Guid.NewGuid(),
                            Name = input.Name,
                            CountryId = input.CountryId,
                            State = input.State

                            // Add other properties as needed
                        };

                        _context.Cities.Add(city);
                        await _context.SaveChangesAsync();

                        response.Message = "City added successfully";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Message = "City with the same name already exists";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    var city = await _context.Cities.FindAsync(input.Id);

                    if (city != null)
                    {
                        city.Name = input.Name;
                        city.CountryId = input.CountryId;
                        city.State = input.State;


                        await _context.SaveChangesAsync();

                        response.Message = "City updated successfully";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Message = "City not found";
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

        public async Task<ClientResponse> GetCities()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var cities = await _context.Cities.Where(x => !x.IsDeleted).Include(x => x.country).Include(x => x.stateId).OrderBy(x => x.Name).ToListAsync();


                if (cities == null )
                {
                    response.Message = "No cities found";
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                    response.HttpResponse = null;
                }
                else
                {
                    response.Message = "cities retrieved successfully";
                    response.HttpResponse = cities;
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
        public async Task<ClientResponse> GetCityByState(Guid stateId)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var cities = await _context.Cities
                                          .Where(x => x.State == stateId && !x.IsDeleted)
                                          .Include(x => x.country)
                                          .Include(x => x.stateId)
                                          .OrderBy(x => x.Name)
                                          .ToListAsync();

                if (cities == null || cities.Count == 0)
                {
                    response.Message = "No cities found for the given StateId";
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                    response.HttpResponse = null;
                }
                else
                {
                    response.Message = "Cities retrieved successfully";
                    response.HttpResponse = cities;
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




        public async Task<ClientResponse> DeleteCity(Guid id)
        {
            ClientResponse response = new();
            try
            {

                var city = await _context.Cities.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (city != null)
                {
                    city.IsDeleted = true;


                    _context.Cities.Update(city);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "City Deleted Failed";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "City Deleted Successfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "City not Exists";
                    response.StatusCode = HttpStatusCode.OK;
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

        static Expression<Func<City, bool>> CombineLambdas(Expression<Func<City, bool>> expr1, Expression<Func<City, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(City));
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

            return Expression.Lambda<Func<City, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterCity(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.Cities.
    Where(x => x.IsDeleted != true)
    .Include(x => x.stateId)
    .Include(x => x.country)
    .AsQueryable();

                // Loop through each filter

                Expression<Func<City, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(City));
                Expression<Func<City, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<City, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<City, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<City, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<City, string>>(SortColumn, parameter);
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
                var City = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseCityDto Response = new ResponseCityDto()
                {
                    city = City,
                    TotalRecord = totalRecord
                };

                if (City == null)
                {
                    response.Message = "No Any City";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "City Get Sucesfully";
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

        public async Task<ClientResponse> GetCityById(Guid id)
        {
            ClientResponse response = new();
            try
            {

                var city = await _context.Cities.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (city != null)
                {

                    response.Message = "City Get Sucesfully";
                    response.HttpResponse = city;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "City not Exists";
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
