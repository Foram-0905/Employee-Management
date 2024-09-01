﻿using beONHR.Entities.DTO;
using beONHR.Entities;
using beONHR.Entities.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using beONHR.Entities.DTO.Enum;
using System.Linq.Expressions;

namespace beONHR.DAL
{
    public interface ICurrencyRepo
    {
        Task<ClientResponse> SaveCurrency(CurrencyDTO input);
        Task<ClientResponse> GetCurrency();
        Task<ClientResponse> GetFilterCurrency(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetCurrencyById(Guid id);
        Task<ClientResponse> DeleteCurrency(Guid id);
    }

    public class CurrencyRepo : ICurrencyRepo
    {
        private readonly MainContext _context;

        public CurrencyRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveCurrency(CurrencyDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var currency = await _context.Currencies.Where(x => x.ShortWord == input.ShortWord && x.IsDeleted != true).FirstOrDefaultAsync();

                    if (currency == null )
                    {
                        Currency model = new Currency
                        {
                            Country = input.Country,
                            ShortWord = input.ShortWord,
                            Symbol = input.Symbol,
                        };

                        await _context.Currencies.AddAsync(model);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0 )
                        {
                            response.Message = "Currency not inserted";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else 
                        {
                            response.Message = "Currency inserted successfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Currency already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    var currency = await _context.Currencies.Where(x => x.Id == input.Id).FirstOrDefaultAsync();

                    if (currency != null)
                    {
                        currency.Country = input.Country;
                        currency.ShortWord = input.ShortWord;
                        currency.Symbol = input.Symbol;

                        _context.Currencies.Update(currency);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Currency not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Currency updated successfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Currency not found";
                        response.StatusCode = HttpStatusCode.NoContent;
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

        public async Task<ClientResponse> GetCurrency()
        {
            ClientResponse response = new();
            try
            {
                var currencies = await _context.Currencies.Include(x => x.countryId).Where(x => x.IsDeleted != true).ToListAsync();

                if (currencies == null || currencies.Count == 0)
                {
                    response.Message = "No currencies found";
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Currencies retrieved successfully";
                    response.HttpResponse = currencies;
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

        public async Task<ClientResponse> DeleteCurrency(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var currency = await _context.Currencies.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (currency != null)
                {
                    currency.IsDeleted = true;


                    _context.Currencies.Update(currency);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "Currency Deleted Failed";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Currency Deleted Successfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Currency not Exists";
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

        public async Task<ClientResponse> GetCurrencyById(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var currency = await _context.Currencies.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (currency != null)
                {

                    response.Message = "Currency Get Sucesfully";
                    response.HttpResponse = currency;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "Currency not Exists";
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



        static Expression<Func<Currency, bool>> CombineLambdas(Expression<Func<Currency, bool>> expr1, Expression<Func<Currency, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(Currency));
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

            return Expression.Lambda<Func<Currency, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterCurrency(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.Currencies.Where(x => x.IsDeleted != true).Include(x => x.countryId).AsQueryable();
                // Loop through each filter

                Expression<Func<Currency, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(Currency));
                Expression<Func<Currency, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<Currency, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<Currency, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<Currency, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<Currency, string>>(SortColumn, parameter);
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
                var currency = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseCurrencyDto Response = new ResponseCurrencyDto()
                {
                    currencies = currency,
                    TotalRecord = totalRecord
                };

                if (currency == null)
                {
                    response.Message = "No Any Currency";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Desigantion Get Currency";
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
