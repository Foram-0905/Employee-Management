using beONHR.Entities.DTO;
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
    public interface IBonusRepo
    {
        Task<ClientResponse> SaveBonus(BonusDTO input);
        Task<ClientResponse> GetBonus();
        Task<ClientResponse> GetFilterBonus(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetBonusById(Guid id);
        Task<ClientResponse> DeleteBonus(Guid id);
    }

    public class BonusRepo : IBonusRepo
    {
        private readonly MainContext _context;

        public BonusRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveBonus(BonusDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var bonus = await _context.Bonus.Where(x => x.Bonusamount == input.Bonusamount).FirstOrDefaultAsync();

                   
                        Bonus model = new Bonus
                        {
                            Entitlement = input.Entitlement,
                            StartingDate = input.StartingDate,
                            EndingDate = input.EndingDate,
                            Bonusamount = input.Bonusamount,
                            SalaryType = input.SalaryType,
                            EmployeeId = input.EmployeeId,
                        };

                        await _context.Bonus.AddAsync(model);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Bonus not inserted";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Bonus inserted successfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                 
                }
                else
                {
                    var bonus = await _context.Bonus.Where(x => x.Id == input.Id).FirstOrDefaultAsync();

                    if (bonus != null)
                    {
                        bonus.Entitlement = input.Entitlement;
                        bonus.StartingDate = input.StartingDate;
                        bonus.EndingDate = input.EndingDate;
                        bonus.Bonusamount = input.Bonusamount;
                        bonus.SalaryType = input.SalaryType;
                        bonus.EmployeeId = input.EmployeeId;

                        _context.Bonus.Update(bonus);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Bonus not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Bonus updated successfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Bonus not found";
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

        public async Task<ClientResponse> GetBonus()
        {
            ClientResponse response = new();
            try
            {
                var bonus = await _context.Bonus.Include(x => x.Salarytype).Where(x => x.IsDeleted != true).ToListAsync();

                if (bonus == null || bonus.Count == 0)
                {
                    response.Message = "No Bonus found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Bonus retrieved successfully";
                    response.HttpResponse = bonus;
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

        public async Task<ClientResponse> DeleteBonus(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var bonus = await _context.Bonus.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (bonus != null)
                {
                    bonus.IsDeleted = true;


                    _context.Bonus.Update(bonus);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "Bonus Deleted Failed";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Bonus Deleted Successfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Bonus not Exists";
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

        public async Task<ClientResponse> GetBonusById(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var bonus = await _context.Bonus.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (bonus != null)
                {

                    response.Message = "Bonus Get Sucesfully";
                    response.HttpResponse = bonus;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "Bonus not Exists";
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



        static Expression<Func<Bonus, bool>> CombineLambdas(Expression<Func<Bonus, bool>> expr1, Expression<Func<Bonus, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(Bonus));
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

            return Expression.Lambda<Func<Bonus, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterBonus(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.Bonus.Where(x => x.IsDeleted != true).Include(x => x.Salarytype).AsQueryable();
                // Loop through each filter

                Expression<Func<Bonus, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(Bonus));
                Expression<Func<Bonus, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<Bonus, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<Bonus, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<Bonus, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<Bonus, string>>(SortColumn, parameter);
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
                var bonus = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseBonussDTO Response = new ResponseBonussDTO()
                {
                    Bonus = bonus,
                    TotalRecord = totalRecord
                };

                if (bonus == null)
                {
                    response.Message = "No Any Bonus";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.NoContent;
                }
                else
                {
                    response.Message = "Bonus Get Successfully";
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
