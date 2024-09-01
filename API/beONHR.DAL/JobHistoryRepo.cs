using beONHR.Entities.Context;
using beONHR.Entities.DTO.Enum;
using beONHR.Entities.DTO;
using beONHR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace beONHR.DAL
{
    public interface IJobHistoryRepo
    {
        Task<ClientResponse> SaveJobHistory(JobHistoryDTO input);
        Task<ClientResponse> GetFilterJobHistory(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetJobHistory();
        Task<ClientResponse> DeleteJobHistory(Guid id);
        Task<ClientResponse> GetJobHistoryById(Guid id);

        Task<ClientResponse> GetJobHistoryByEmployeeId(Guid id);
    }

    public class JobHistoryRepo : IJobHistoryRepo
    {
        private readonly MainContext _context;

        public JobHistoryRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveJobHistory(JobHistoryDTO input)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var existingJobProfile = await _context.JobHistories.FirstOrDefaultAsync(x => x.CompanyName == input.CompanyName && x.IsDeleted != true);

                    if (existingJobProfile == null)
                    {
                        var JobHistory = new JobHistory
                        {
                            Id = Guid.NewGuid(),
                            CompanyName = input.CompanyName,
                            PositionHeld = input.PositionHeld,
                            EmploymentType = input.EmploymentType,
                            ZipCode = input.ZipCode,
                            City = input.City,
                            Country = input.Country,
                            StartDate = input.StartDate,
                            EndDate = input.EndDate,
                            Document = input.Document,
                            LeavingReason = input.LeavingReason,
                            EmployeeId = input.EmployeeId,
                            StateId = input.StateId,

                            // Add other properties as needed
                        };

                        _context.JobHistories.Add(JobHistory);
                        //DocumentList document = new DocumentList
                        //{
                        //    Id = Guid.NewGuid(),
                        //    TabName = "Job-History",
                        //    Modulename = "Job-History",
                        //    FileName = input.Filename,
                        //    Documents = JobHistory.Document,
                        //    EmployeeId = input.EmployeeId,
                        //};

                        //_context.DocumentList.Add(document);
                        var res = await _context.SaveChangesAsync();
                        if (res != 0)
                        {

                            response.Message = "Job History added successfully";
                            response.StatusCode = HttpStatusCode.OK;
                            response.IsSuccess = true;
                        }
                    }
                    else
                    {
                        response.Message = "Job-History with the same name already exists";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    var jobHistory = await _context.JobHistories.FindAsync(input.Id);

                    if (jobHistory != null)
                    {
                        jobHistory.CompanyName = input.CompanyName;
                        jobHistory.PositionHeld = input.PositionHeld;
                        jobHistory.EmploymentType = input.EmploymentType;
                        jobHistory.ZipCode = input.ZipCode;
                        jobHistory.City = input.City;
                        jobHistory.Country = input.Country;
                        jobHistory.StartDate = input.StartDate;
                        jobHistory.EndDate = input.EndDate;
                        jobHistory.Document = input.Document;
                        jobHistory.LeavingReason = input.LeavingReason;
                        jobHistory.EmployeeId = input.EmployeeId;
                        jobHistory.StateId = input.StateId;


                     //   var existingDocument = await _context.DocumentList
                     //.FirstOrDefaultAsync(d => d.EmployeeId == input.EmployeeId && d.Modulename == "Job-History");

                     //   if (existingDocument != null)
                     //   {
                     //       // Update the document list record
                     //       existingDocument.Id = existingDocument.Id;
                     //       existingDocument.FileName = input.Filename;
                     //       existingDocument.Documents = jobHistory.Document;
                     //       existingDocument.TabName = "Job-History";
                     //       existingDocument.Modulename = "Job-History";
                     //       existingDocument.EmployeeId = input.EmployeeId;
                     //       _context.DocumentList.Update(existingDocument);
                     //   }


                        await _context.SaveChangesAsync();

                        response.Message = "jobHistory updated successfully";
                        response.StatusCode = HttpStatusCode.OK;
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Message = "jobHistory not found";
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

        public async Task<ClientResponse> GetJobHistory()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var jobhistory = await _context.JobHistories.Where(x => !x.IsDeleted).Include(x => x.EmployeeType).Include(x => x.countryId).Include(x => x.State).Include(x => x.cityId).Include(x => x.Employee)
                    .Select(e => new JobHistoryDTO
                {
                        Id = e.Id,
                        CompanyName = e.CompanyName,
                        PositionHeld = e.PositionHeld,
                        EmploymentType = e.EmploymentType,
                        EmploymentTypeName = e.EmployeeType.typeofemployment,
                        ZipCode = e.ZipCode,
                        City = e.City,
                        CityName = e.cityId.Name,
                        StateId = e.StateId,
                        StateName = e.State.Name,
                        Country = e.Country,
                        CountryName = e.countryId.CountryName,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        //Document=e.Document,
                        LeavingReason = e.LeavingReason,
                        EmployeeId = e.EmployeeId,

                    }).ToListAsync();

                if (jobhistory != null && jobhistory.Count > 0)
                {
                    response.Message = "jobhistory retrieved successfully";
                    response.HttpResponse = jobhistory;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "No jobhistory found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteJobHistory(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var jobhistory = await _context.JobHistories.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (jobhistory != null)
                {
                    jobhistory.IsDeleted = true;
                    _context.JobHistories.Update(jobhistory);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "jobhistory Deleted Faild";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "jobhistory Deleted Sucesfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "jobhistory not Exists";
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



        static Expression<Func<JobHistory, bool>> CombineLambdas(Expression<Func<JobHistory, bool>> expr1, Expression<Func<JobHistory, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(JobHistory));
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

            return Expression.Lambda<Func<JobHistory, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterJobHistory(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.JobHistories.Where(x => x.IsDeleted != true).Include(x => x.countryId).Include(x => x.EmployeeType).Include(x => x.State).Include(x => x.cityId).Include(x => x.Employee).AsQueryable();
                // Loop through each filter

                Expression<Func<JobHistory, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(JobHistory));
                Expression<Func<JobHistory, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<JobHistory, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<JobHistory, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<JobHistory, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<JobHistory, string>>(SortColumn, parameter);
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
                var JobHistory = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).Select(e => new JobHistoryDTO
                            {
                                Id = e.Id,
                                CompanyName = e.CompanyName,
                                PositionHeld = e.PositionHeld,
                                EmploymentType = e.EmploymentType,
                                EmploymentTypeName=e.EmployeeType.typeofemployment,
                                ZipCode = e.ZipCode,
                                City = e.City,
                                CityName=e.cityId.Name,
                                StateId = e.StateId,
                                StateName=e.State.Name,
                                Country = e.Country,
                                CountryName=e.countryId.CountryName,
                                StartDate = e.StartDate,
                                EndDate = e.EndDate,
                                //Document=e.Document,
                                LeavingReason = e.LeavingReason,
                                EmployeeId = e.EmployeeId,

                            }).ToListAsync();

                ResponseJobHistoyDTO Response = new ResponseJobHistoyDTO()
                {
                    JobHistory = JobHistory,
                    TotalRecord = totalRecord
                };

                if (JobHistory == null)
                {
                    response.Message = "No Any JobHistory";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "JobHistory Get Sucesfully";
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

        public async Task<ClientResponse> GetJobHistoryById(Guid id)
        {
            ClientResponse response = new();
            try
            {

                var jobhistory = await _context.JobHistories.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (jobhistory != null)
                {

                    response.Message = "JobHistories Get Sucesfully";
                    response.HttpResponse = jobhistory;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "JobHistories not Exists";
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


        public async Task<ClientResponse> GetJobHistoryByEmployeeId(Guid id)
        {
            ClientResponse response = new();
            try
            {
                var jobhistories = await _context.JobHistories.Where(x => x.EmployeeId == id && x.IsDeleted != true).Include(X => X.EmployeeType).Include(X => X.cityId).Include(X => X.countryId).Include(X => X.State).ToListAsync();

                if (jobhistories != null && jobhistories.Count > 0)
                {
                    response.Message = "JobHistories retrieved successfully";
                    response.HttpResponse = jobhistories;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "No job histories found for the specified employee.";
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

