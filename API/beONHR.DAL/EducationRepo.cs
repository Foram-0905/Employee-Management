// EducationRepo.cs
using beONHR.Entities.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using beONHR.Entities;
using beONHR.Entities.Context;
using System.Collections.Generic;
using System.Linq.Expressions;
using beONHR.Entities.DTO.Enum;

namespace beONHR.DAL
{
    public interface IEducationRepo
    {
        Task<ClientResponse> SaveEducation(EducationDTO input);
        Task<ClientResponse> GetEducation();
        Task<ClientResponse> GetFilterEducation(FilterRequsetDTO filterRequset);
        Task<ClientResponse> DeleteEducation(Guid id);
        Task<ClientResponse> GetEducationById(Guid id);
        Task<ClientResponse> GetEducationByEmployee(Guid employeeId); // Add this line

    }

    public class EducationRepo : IEducationRepo
    {
        private readonly MainContext _context;

        public EducationRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveEducation(EducationDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input == null)
                {
                    response.Message = "Education data is null";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    return response;
                }

                // Check if the asset with the given ID exists
                var existingEducation = await _context.Educations.FindAsync(input.Id);

                if (existingEducation == null)
                {
                    // Create a new asset
                    Education edu = new Education
                    {
                        EducationLevels = input.EducationLevels,
                        Subject = input.Subject,
                        Institute = input.Institute,
                        City = input.City,
                        State=input.State,
                        Country = input.Country,
                        CompletionDate = input.CompletionDate,
                        Certificate = input.Certificate,
                        Anabin = input.Anabin,
                        Employee= input.Employee,
                    };

                    _context.Educations.Add(edu);
                    //DocumentList document = new DocumentList
                    //{
                    //    Id = Guid.NewGuid(),
                    //    TabName = "Education",
                    //    Modulename = "Education",
                    //    FileName = input.Filename,
                    //    Documents = edu.Certificate,
                    //    EmployeeId = input.Employee,
                    //};



                    //_context.DocumentList.Add(document);
                    var res = await _context.SaveChangesAsync();


                    response.Message = "Education saved successfully";
                    response.HttpResponse = edu.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }
                else
                {
                    // Update existing asset
                    existingEducation.EducationLevels = input.EducationLevels;
                    existingEducation.Subject = input.Subject;
                    existingEducation.Institute = input.Institute;
                    existingEducation.City = input.City;
                    existingEducation.State = input.State;
                    existingEducation.Country = input.Country;
                    existingEducation.CompletionDate = input.CompletionDate;
                    existingEducation.Certificate = input.Certificate;
                    existingEducation.Anabin = input.Anabin;
                    existingEducation.Employee = input.Employee;

                    _context.Educations.Update(existingEducation);

                    //var existingDocument = await _context.DocumentList
                    // .FirstOrDefaultAsync(d => d.EmployeeId == input.Employee && d.Modulename == "Education");

                    //if (existingDocument != null)
                    //{
                    //    // Update the document list record
                    //    existingDocument.Id = existingDocument.Id;
                    //    existingDocument.FileName = input.Filename;
                    //    existingDocument.Documents = existingEducation.Certificate;
                    //    existingDocument.Modulename = "Education";
                    //    existingDocument.EmployeeId= input.Employee;
                    //    _context.DocumentList.Update(existingDocument);
                    //}
                    await _context.SaveChangesAsync();

                    response.Message = "Education updated successfully";
                    response.HttpResponse = existingEducation.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetEducation()
        {
            ClientResponse response = new();
            try
            {
                // Get all Education
                var education = await _context.Educations
                    .Where(x => x.IsDeleted != true)
                    .Select(e => new EducationDTO
                    {
                        Id = e.Id,
                        EducationLevels = e.EducationLevels,
                        Subject = e.Subject,
                        Institute = e.Institute,
                        City = e.City,
                        State = e.State,
                        Country = e.Country,
                        CompletionDate = e.CompletionDate,
                        EducationLevelName = e.EducationLevelid.Level,
                        //Certificate = e.Certificate,
                        //Anabin = e.Anabin,
                        Employee = e.Employee,
                        // Add more properties as needed
                    })
                    .ToListAsync();

                response.Message = "Education retrieved successfully";
                response.HttpResponse = education;
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public async Task<ClientResponse> DeleteEducation(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var education = await _context.Educations.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (education != null)
                {
                    education.IsDeleted = true;


                    _context.Educations.Update(education);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "Education Deleted Failed";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Education Deleted Successfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Education not Exists";
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

        public async Task<ClientResponse> GetEducationById(Guid id)
        {
            ClientResponse response = new();
            try
            {
                // Get the asset by ID
                var education = await _context.Educations.FindAsync(id);

                if (education == null)
                {
                    response.Message = "education not found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "education retrieved successfully";
                    response.HttpResponse = education;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetEducationByEmployee(Guid employeeId)
        {
            ClientResponse response = new();
            try
            {
                var education = await _context.Educations
                    .Where(e => e.Employee == employeeId && e.IsDeleted != true)
                    .Select(e => new EducationDTO
                    {
                        Id = e.Id,
                        EducationLevels = e.EducationLevels,
                        Subject = e.Subject,
                        Institute = e.Institute,
                        City = e.City,
                        State = e.State,
                        Country = e.Country,
                        CompletionDate = e.CompletionDate,
                        EducationLevelName = e.EducationLevelid.Level,
                        Employee = e.Employee,
                        // Add more properties as needed
                    })
                    .ToListAsync();

                if (education == null || education.Count == 0)
                {
                    response.Message = "No education records found for the given employee";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Education records retrieved successfully";
                    response.HttpResponse = education;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        static Expression<Func<Education, bool>> CombineLambdas(Expression<Func<Education, bool>> expr1, Expression<Func<Education, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(Education));
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

            return Expression.Lambda<Func<Education, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterEducation(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.Educations
                    .Where(x => x.IsDeleted != true)
                    .Include(x => x.countryid)
                    .Include(a => a.stateid)
                    .Include(a => a.cityid)
                    .Include(a => a.EducationLevelid)
                    .AsQueryable();

                // Loop through each filter
                Expression<Func<Education, bool>> combinedCondition = null;
                var parameter = Expression.Parameter(typeof(Education));
                Expression<Func<Education, bool>> lambda = null;

                if (filterRequset.filterModel != null && filterRequset.filterModel.Count() != 0)
                {
                    foreach (var filter in filterRequset.filterModel)
                    {
                        var property = Expression.Property(parameter, filter.Key);
                        var value = Expression.Constant(filter.Value.filter);
                        var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                        lambda = Expression.Lambda<Func<Education, bool>>(condition, parameter);

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

                var totalRecord = await query.CountAsync();
                int skip = (int)((filterRequset.PageNumber - 1) * filterRequset.PageSize);

                var education = await query
                     .Include(e => e.EducationLevelid) // Include EducationLevel
                    .Include(e => e.cityid)           // Include City
                    .Include(e => e.stateid)          // Include State
                    .Include(e => e.countryid)
                    .Skip(skip)
                    .Take((int)(filterRequset.PageSize))
                    .Select(e => new EducationDTO
                    {
                        Id = e.Id,
                        EducationLevels = e.EducationLevels,
                        Subject = e.Subject,
                        Institute = e.Institute,
                        City = e.City,
                        State = e.State,
                        Country = e.Country,
                        CompletionDate = e.CompletionDate,
                        EducationLevelName =e.EducationLevelid.Level,
                        //Certificate = e.Certificate,
                        //Anabin = e.Anabin,
                        Employee = e.Employee,
                        // Add more properties as needed
                    })
                    .ToListAsync();

                ResponseEducationDto Response = new ResponseEducationDto()
                {
                    Education = education,
                    TotalRecord = totalRecord
                };

                if (education == null || !education.Any())
                {
                    response.Message = "No Education found";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Education retrieved successfully";
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
