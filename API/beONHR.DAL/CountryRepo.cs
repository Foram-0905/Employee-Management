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
    public interface ICountryRepo
    {
        Task<ClientResponse> SaveCountry(CountryDTO input);
        Task<ClientResponse> GetCountries();
        Task<ClientResponse> DeleteCountry(Guid countryId);
        Task<ClientResponse> GetCountry();
        Task<ClientResponse> GetFilterCountry(FilterRequsetDTO filterRequset);

        Task<ClientResponse> GetCountryById(Guid Id);
    }

    public class CountryRepo : ICountryRepo
    {
        private readonly MainContext _context;

        public CountryRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveCountry(CountryDTO input)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var country = await _context.Countries.FirstOrDefaultAsync(x => x.CountryName == input.CountryName);

                    if (country == null)
                    {
                        Country model = new Country
                        {
                            Id = Guid.NewGuid(), // Automatically generate GUID
                            CountryName = input.CountryName
                        };

                        await _context.Countries.AddAsync(model);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Country not saved";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Country saved successfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Country already exists";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = false;
                    }

                    return response;
                }
                else
                {
                    var country = await _context.Countries.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefaultAsync();

                    if (country != null)
                    {
                        country.Id = input.Id;
                        country.CountryName = input.CountryName;
                       


                        _context.Countries.Update(country);
                        var res = await _context.SaveChangesAsync();


                        if (res == 0)
                        {
                            response.Message = "Country not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Country updated Sucesfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Country Not Exists";
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

        public async Task<ClientResponse> GetCountry()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var countries = await _context.Countries.Where(x => x.IsDeleted != true).OrderBy(x => x.CountryName).ToListAsync();

                if (countries == null )
                {
                    response.Message = "No Any countries";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Countries retrieved successfully";
                    response.HttpResponse = countries;
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
        public async Task<ClientResponse> GetCountryById(Guid Id)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var country = await _context.Countries.FindAsync(Id);

                if (country != null)
                {
                    response.Message = "Country retrieved successfully";
                    response.HttpResponse = country;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Country not found";
                    response.StatusCode = HttpStatusCode.OK;
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



        static Expression<Func<Country, bool>> CombineLambdas(Expression<Func<Country, bool>> expr1, Expression<Func<Country, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(Country));
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

            return Expression.Lambda<Func<Country, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterCountry(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.Countries.Where(x => x.IsDeleted != true).AsQueryable();
                // Loop through each filter

                Expression<Func<Country, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(Country));
                Expression<Func<Country, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<Country, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<Country, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<Country, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<Country, string>>(SortColumn, parameter);
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
                var country = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseCountryDto Response = new ResponseCountryDto()
                {
                    country = country,
                    TotalRecord = totalRecord
                };

                if (country == null)
                {
                    response.Message = "No Any country";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "country Get Sucesfully";
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
        public async Task<ClientResponse> DeleteCountry(Guid countryId)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var country = await _context.Countries.FindAsync(countryId);

                if (country != null)
                {
                    _context.Countries.Remove(country);
                    await _context.SaveChangesAsync();

                    response.Message = "Country deleted successfully";
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Country not found";
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

        public Task<ClientResponse> GetCountries()
        {
            throw new NotImplementedException();
        }
    }
}
