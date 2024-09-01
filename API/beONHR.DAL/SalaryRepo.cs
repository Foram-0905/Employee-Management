using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Email;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace beONHR.DAL
{
    public interface ISalaryRepo
    {
        Task<ClientResponse> SaveSalary(SalaryDTO input);
        Task<ClientResponse> GetSalary();
      
        Task<ClientResponse> DeleteSalary(Guid id);
        Task<ClientResponse> GetPreviousMonthSalary(Guid id);
        Task<ClientResponse> GetTwoMonthsAgoSalary(Guid id);
        Task<ClientResponse> GetFilterSalary(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetSalaryById(Guid id);
        Task<ClientResponse> GetSalaryByEmployee(Guid id);


    }
    public class SalaryRepo : ISalaryRepo
    {
        private readonly MainContext _context;
        private readonly IEmailRepo _emailRepo;

        public SalaryRepo(MainContext context)
        {
            _context = context;
            
        }
        public async Task<ClientResponse> SaveSalary(SalaryDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var salary = await _context.EmpSalary
                        .Where(x => x.SalaryType == input.SalaryType && x.EmployeeId == input.EmployeeId && x.Currency == input.Currency && x.Amount == input.Amount && x.TransactionType == input.TransactionType && x.IsDeleted != true)
                        .FirstOrDefaultAsync();

                    if (salary == null)
                    {
                        Salary model = new Salary
                        {
                            Amount = input.Amount,
                            SalaryStartDate = input.SalaryStartDate,
                            SalaryEndDate = input.SalaryEndDate,
                            Currency = input.Currency,
                            SalaryType = input.SalaryType,
                            EmployeeId = input.EmployeeId,
                            TransactionType = input.TransactionType,
                        };

                        await _context.EmpSalary.AddAsync(model);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "EmpSalary not inserted";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "EmpSalary inserted successfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;

                            // Send notification
                            
                        }
                    }
                    else
                    {
                        response.Message = "EmpSalary already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }
                    return response;
                }
                else
                {
                    var salary = await _context.EmpSalary
                        .Where(x => x.Id == input.Id && x.IsDeleted == false)
                        .FirstOrDefaultAsync();

                    if (salary != null)
                    {
                        salary.Id = input.Id;
                        salary.Amount = input.Amount;
                        salary.Currency = input.Currency;
                        salary.SalaryType = input.SalaryType;
                        salary.SalaryStartDate = input.SalaryStartDate;
                        salary.SalaryEndDate = input.SalaryEndDate;
                        salary.EmployeeId = input.EmployeeId;
                        salary.TransactionType = input.TransactionType;
                        _context.EmpSalary.Update(salary);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "EmpSalary not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "EmpSalary updated successfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;

                            
                        }
                    }
                    else
                    {
                        response.Message = "EmpSalary Not Exists";
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

      







        public async Task<ClientResponse> GetSalary()
        {
            ClientResponse response = new ClientResponse();
    
            try
            {
                // Current date
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today);


                var salary = await _context.EmpSalary.Include(x => x.SalaryTypeName).Include(x=> x.TransactionTypeName).Include(x=>x.Currenccy).Include(x=>x.Employee).Include(x => x.TransactionTypeName).Where(x => x.SalaryStartDate.Month == currentDate.Month && x.SalaryStartDate.Year == currentDate.Year && x.IsDeleted != true).ToListAsync();
                
                // Calculate the total salary amount
                double totalSalary = salary.Sum(x => x.Amount);
             

                Salary newSalary = new Salary
                {
                    Id = Guid.NewGuid(), // Generate a new unique ID
                    Amount = totalSalary, // Set the amount from input
                };
                //newSalary.SalaryEndDate = null; 
                salary.Add(newSalary);

                if (salary == null)
                {
                    response.Message = "No Any salary";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                response.Message = "salary Get Sucesfully";
              
                response.HttpResponse = salary;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                //response.HttpResponse.
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ClientResponse> DeleteSalary(Guid id)
        {

            ClientResponse response = new ClientResponse();
            try
            {

                var salary = await _context.EmpSalary.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (salary != null)
                {
                    salary.IsDeleted = true;
                    _context.EmpSalary.Update(salary);
                    var res = await _context.SaveChangesAsync();

                    if (res == 0)
                    {
                        response.Message = "Salary Deleted Faild";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Salary Deleted Sucesfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Salary not Exists";
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


        public async Task<ClientResponse> GetPreviousMonthSalary(Guid id)
        {
            ClientResponse response = new ClientResponse();

            try
            {
                // Calculate previous month date
                DateOnly previousMonthDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(-1));

                // Get data for previous month
                var salary = await _context.EmpSalary
                    .Include(x => x.SalaryTypeName)
                    .Include(x => x.Currenccy)
                    .Where(x => x.EmployeeId == id && x.SalaryStartDate.Month == previousMonthDate.Month && x.SalaryStartDate.Year == previousMonthDate.Year && x.IsDeleted != true)
                    .ToListAsync();

                if (salary == null || salary.Count == 0)
                {
                    response.Message = "No salary data available for the previous month.";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Previous month salary data retrieved successfully.";
                    response.HttpResponse = salary;
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

        public async Task<ClientResponse> GetTwoMonthsAgoSalary(Guid id)
        {
            ClientResponse response = new ClientResponse();

            try
            {
                // Calculate two months ago date
                DateOnly twoMonthsAgoDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(-2));

                // Get data for two months ago
                var salary = await _context.EmpSalary
                    .Include(x => x.SalaryTypeName)
                    .Include(x => x.Employee)
                    .Include(x => x.Currenccy)
                    .Where(x => x.EmployeeId == id && x.SalaryStartDate.Month == twoMonthsAgoDate.Month && x.SalaryStartDate.Year == twoMonthsAgoDate.Year && x.IsDeleted != true)
                    .ToListAsync();

                if (salary == null || salary.Count == 0)
                {
                    response.Message = "No salary data available for two months ago.";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Two months ago salary data retrieved successfully.";
                    response.HttpResponse = salary;
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



        static Expression<Func<Salary, bool>> CombineLambdas(Expression<Func<Salary, bool>> expr1, Expression<Func<Salary, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(Salary));
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

            return Expression.Lambda<Func<Salary, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterSalary(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.EmpSalary.Where(x => x.IsDeleted != true).Include(x => x.Currenccy).Include(x => x.SalaryTypeName).Include(x => x.TransactionTypeName).Include(x => x.Employee).AsQueryable();
                // Loop through each filter

                Expression<Func<Salary, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(Salary));
                Expression<Func<Salary, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<Salary, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<Salary, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<Salary, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<Salary, string>>(SortColumn, parameter);
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
                var salary = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseSalaryDto Response = new ResponseSalaryDto()
                {
                    salary = salary,
                    TotalRecord = totalRecord
                };

                if (salary == null)
                {
                    response.Message = "No Any Salary";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Salary Get Sucesfully";
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

        public async Task<ClientResponse> GetSalaryByEmployee(Guid id)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                // Get the current date
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today);

                var salary = await _context.EmpSalary
                    .Include(x => x.SalaryTypeName)
                    .Include(x => x.TransactionTypeName)
                    .Include(x => x.Currenccy)
                    .Include(x => x.Employee)
                    .Where(x => x.EmployeeId == id &&
                                x.SalaryStartDate.Month == currentDate.Month &&
                                x.SalaryStartDate.Year == currentDate.Year &&
                                x.IsDeleted != true)
                    .ToListAsync();

                if (salary != null && salary.Any())
                {
                    // Calculate the total salary amount
                    double totalSalary = salary.Sum(x => x.Amount);

                    // Create a new salary object
                    //Salary payableSalary = new Salary();
                    //payableSalary.Amount= totalSalary;
                    var firstSalary = salary.First();
                    Salary payableSalary = new Salary
                    {
                        Amount = totalSalary,
                        Currenccy = firstSalary.Currenccy,
                        

                        // other fields as needed
                    };


                    salary.Add(payableSalary);
                    // Add the new salary to the existing salary list
                    //salary.Add(newSalary);

                    response.Message = "Salaries retrieved successfully";
                    response.HttpResponse = salary;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Salaries do not exist for the specified employee";
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = false;
                    response.HttpResponse = null;

                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<ClientResponse> GetSalaryById(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var salary = await _context.EmpSalary.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (salary != null)
                {

                    response.Message = "Salary Get Sucesfully";
                    response.HttpResponse = salary;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "Salary not Exists";
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
