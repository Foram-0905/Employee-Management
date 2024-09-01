using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface IPublicHolidayRepo
    {
        Task<ClientResponse> SavePublicHoliday(PublicHolidayDTO input);
        Task<ClientResponse> GetPublicHoliday();
        Task<ClientResponse> DeletePublicHoliday(Guid id);
        Task<ClientResponse> GetPublicHolidayById(Guid id);
        Task<ClientResponse> GetFilterPublicHoliday(FilterRequsetDTO filterRequset);
    }
    public class PublicHolidayRepo : IPublicHolidayRepo
    {
        private readonly MainContext _context;
        public PublicHolidayRepo(MainContext context)
        {
            _context = context;
        }
        public async Task<ClientResponse> SavePublicHoliday(PublicHolidayDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var existingHoliday = await _context.PublicHolidays
                        .Where(x => x.Country == input.Country && x.State == input.State && x.HolidayName == input.HolidayName && x.HolidayDate == input.HolidayDate && x.IsDeleted != true)
                        .FirstOrDefaultAsync();

                    if (existingHoliday == null)
                    {
                        PublicHoliday model = new PublicHoliday
                        {
                            Country = input.Country,
                            State = input.State,
                            HolidayName = input.HolidayName,
                            HolidayDate = input.HolidayDate
                        };

                        await _context.PublicHolidays.AddAsync(model);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Public holiday not inserted";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Public holiday inserted successfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Public holiday already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }
                    return response;
                }
                else
                {
                    var holiday = await _context.PublicHolidays
                        .Where(x => x.Id == input.Id && x.IsDeleted == false)
                        .FirstOrDefaultAsync();

                    if (holiday != null)
                    {
                        holiday.Id = input.Id;
                        holiday.Country = input.Country;
                        holiday.State = input.State;
                        holiday.HolidayName = input.HolidayName;
                        holiday.HolidayDate = input.HolidayDate;

                        _context.PublicHolidays.Update(holiday);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Public holiday not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Public holiday updated successfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Public holiday does not exist";
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

        public async Task<ClientResponse> GetPublicHoliday()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var holidays = await _context.PublicHolidays
                    .Where(x => x.IsDeleted != true)
                    .Include(x => x.countryId)
                    .Include(x => x.stateId)
                    .ToListAsync();

                if (holidays == null || holidays.Count == 0)
                {
                    response.Message = "No public holidays found";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Public holidays retrieved successfully";
                    response.HttpResponse = holidays;
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



        static Expression<Func<PublicHoliday, bool>> CombineLambdas(Expression<Func<PublicHoliday, bool>> expr1, Expression<Func<PublicHoliday, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(PublicHoliday));
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

            return Expression.Lambda<Func<PublicHoliday, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterPublicHoliday(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.PublicHolidays.Where(x => x.IsDeleted != true).Include(x => x.countryId).Include(x => x.stateId).AsQueryable();
                // Loop through each filter

                Expression<Func<PublicHoliday, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(PublicHoliday));
                Expression<Func<PublicHoliday, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<PublicHoliday, bool>>(condition, parameter);

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
                                lambda = Expression.Lambda<Func<PublicHoliday, bool>>(condition, parameter);
                            }
                            else
                            {

                                var value = Expression.Constant(filter.Value.filter);
                                var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                                lambda = Expression.Lambda<Func<PublicHoliday, bool>>(condition, parameter);
                            }

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
                if (filterRequset.sortModel != null)
                {
                    if (filterRequset.sortModel.colId.IndexOf(".") != -1)
                    {
                        // for Make Key and Table subString
                        var SortKey = filterRequset.sortModel.colId.Substring(filterRequset.sortModel.colId.IndexOf(".") + 1).ToString();
                        var ForignKeyTable = filterRequset.sortModel.colId.Substring(0, filterRequset.sortModel.colId.IndexOf(".")).ToString();

                        //make exprssion
                        var ForignKeyTableColumn = Expression.Property(parameter, ForignKeyTable);

                        var sortTableCol = Expression.Property(ForignKeyTableColumn, SortKey);

                        var ForginTableInclude = Expression.Lambda(sortTableCol, parameter);

                        var sortCodition = Expression.Lambda<Func<PublicHoliday, string>>(sortTableCol, parameter);

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

                        var SortColumn = Expression.Property(parameter, filterRequset.sortModel.colId);
                        var sortCodition = Expression.Lambda<Func<PublicHoliday, string>>(SortColumn, parameter);
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
                }
                else
                {

                    query = query;
                }
                var totalRecord = query.Count();
                int skip = (int)((filterRequset.PageNumber - 1) * filterRequset.PageSize);
                //int last =
                var publicHoliday = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponsePublicHolidayDto Response = new ResponsePublicHolidayDto()
                {
                    publicHoliday = publicHoliday,
                    TotalRecord = totalRecord
                };

                if (publicHoliday == null)
                {
                    response.Message = "No Any PublicHoliday";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "PublicHoliday Get Sucesfully";
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
        public async Task<ClientResponse> DeletePublicHoliday(Guid id)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var holiday = await _context.PublicHolidays
                    .Where(x => x.Id == id && x.IsDeleted != true)
                    .FirstOrDefaultAsync();

                if (holiday != null)
                {
                    holiday.IsDeleted = true;
                    _context.PublicHolidays.Update(holiday);
                    var res = await _context.SaveChangesAsync();

                    if (res == 0)
                    {
                        response.Message = "Failed to delete public holiday";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Public holiday deleted successfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Public holiday does not exist";
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

        public async Task<ClientResponse> GetPublicHolidayById(Guid id)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var holiday = await _context.PublicHolidays
                    .Where(x => x.Id == id && x.IsDeleted != true)
                    .FirstOrDefaultAsync();

                if (holiday != null)
                {
                    response.Message = "Public holiday retrieved successfully";
                    response.HttpResponse = holiday;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Public holiday does not exist";
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