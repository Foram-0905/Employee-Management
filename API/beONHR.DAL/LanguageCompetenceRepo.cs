//using beONHR.Entities;
//using beONHR.Entities.Context;
//using beONHR.Entities.DTO;
//using beONHR.Entities.DTO.Enum;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

//namespace beONHR.DAL
//{
    
//    public interface ILanguageCompetenceRepo
//    {
//        Task<ClientResponse> LanguageCompetenceGetByEmployeeIdAsync(Guid employeeId);

//        Task<ClientResponse> SaveLanguageCompetence(LanguageCompetenceDTO input);
//        Task<ClientResponse> GetLanguageCompetence();
//        Task<ClientResponse> DeleteLanguageCompetence(Guid Id);
        
//    }
//    public class LanguageCompetenceRepo: ILanguageCompetenceRepo
//    {
//        private readonly MainContext _context;

//        public LanguageCompetenceRepo(MainContext context)
//        {
//            _context = context;
//        }

//        public async Task<ClientResponse> SaveLanguageCompetence(LanguageCompetenceDTO input)
//        {
//            ClientResponse response = new ClientResponse();
//            try
//            {
//                if (input.Action == ActionEnum.Insert)
//                {
//                    var languageCompetence = await _context.LanguageCompetences.Where(x => x.Level == input.Level && x.Name==input.Name && x.LanguagesCertificate==input.LanguagesCertificate && x.EmployeeId == input.EmployeeId && x.IsDeleted != true).FirstOrDefaultAsync();

//                    if (languageCompetence == null)
//                    {
//                        LanguageCompetence model = new LanguageCompetence
//                        {

//                            Level = input.Level ,
//                            Name=input.Name,
//                            LanguagesCertificate=input.LanguagesCertificate,
//                            EmployeeId = input.EmployeeId,
//                            // Add other properties as needed
//                        };

//                        await _context.LanguageCompetences.AddAsync(model);
//                        var res = await _context.SaveChangesAsync();

//                        response.Message = "LanguageCompetence added successfully";
//                        response.StatusCode = HttpStatusCode.OK;
//                        response.IsSuccess = true;
//                    }
//                    else
//                    {
//                        response.Message = "LanguageCompetence with the same name already exists";
//                        response.StatusCode = HttpStatusCode.BadRequest;
//                        response.IsSuccess = false;
//                    }
//                }
//                else
//                {
//                    var languageCompetence = await _context.LanguageCompetences.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefaultAsync();

//                    if (languageCompetence != null)
//                    { 
                       
//                        languageCompetence.Id = input.Id;
//                        languageCompetence.LanguagesCertificate = input.LanguagesCertificate;
//                        languageCompetence.Level = input.Level;
//                        languageCompetence.Name = input.Name;
//                        languageCompetence.EmployeeId = input.EmployeeId;
//                        await _context.SaveChangesAsync();

//                        response.Message = "LanguageCompetence updated successfully";
//                        response.StatusCode = HttpStatusCode.OK;
//                        response.IsSuccess = true;
//                    }
//                    else
//                    {
//                        response.Message = "LanguageCompetence not found";
//                        response.StatusCode = HttpStatusCode.NotFound;
//                        response.IsSuccess = false;
//                    }
//                }

//                return response;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<ClientResponse> LanguageCompetenceGetByEmployeeIdAsync(Guid employeeId)
//        {
//            ClientResponse response = new ClientResponse();
//            try
//            {
//                var languageCompetences = await _context.LanguageCompetences.Include(x => x.Employee).Include(x => x.LanguageLevel)
//                    .Where(x => x.EmployeeId == employeeId && x.IsDeleted != true)
//                    .ToListAsync();

//                if (languageCompetences == null || !languageCompetences.Any())
//                {
//                    response.Message = "No LanguageCompetence found for the given EmployeeId";
//                    response.HttpResponse = null;
//                    response.IsSuccess = false;
//                    response.StatusCode = HttpStatusCode.NotFound;
//                }
//                else
//                {
//                    response.Message = "LanguageCompetence retrieved successfully";
//                    response.HttpResponse = languageCompetences;
//                    response.IsSuccess = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }

//                return response;
//            }
//            catch (Exception ex)
//            {
//                // Handle exceptions here
//                throw ex;
//            }
//        }


//        public async Task<ClientResponse> GetLanguageCompetence()
//        {
//            ClientResponse response = new ClientResponse();
//            try
//            {
//                var languageCompetence = await _context.LanguageCompetences.Include(x => x.Employee).Include(x => x.LanguageLevel).Where(x => x.IsDeleted != true).ToListAsync();

//                if (languageCompetence == null)
//                {
//                    response.Message = "No Any LanguageCompetence";
//                    response.HttpResponse = null;
//                    response.IsSuccess = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }

//                response.Message = "LanguageCompetence Get Sucesfully";
//                response.HttpResponse = languageCompetence;
//                response.IsSuccess = true;
//                response.StatusCode = HttpStatusCode.OK;

//                return response;
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//        }

//        public async Task<ClientResponse> DeleteLanguageCompetence(Guid id)
//        {

//            ClientResponse response = new();
//            try
//            {

//                var languageCompetence = await _context.LanguageCompetences.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

//                if (languageCompetence != null)
//                {
//                    languageCompetence.IsDeleted = true;
//                    _context.LanguageCompetences.Update(languageCompetence);
//                    var res = await _context.SaveChangesAsync();


//                    if (res == 0)
//                    {
//                        response.Message = "LanguageCompetences Deleted Faild";
//                        response.StatusCode = HttpStatusCode.NoContent;
//                        response.IsSuccess = false;
//                    }
//                    else
//                    {
//                        response.Message = "LanguageCompetences Deleted Sucesfully";
//                        response.HttpResponse = null;
//                        response.IsSuccess = true;
//                        response.StatusCode = HttpStatusCode.OK;
//                    }
//                }
//                else
//                {
//                    response.Message = "LanguageCompetences not Exists";
//                    response.StatusCode = HttpStatusCode.NoContent;
//                    response.IsSuccess = false;
//                }

//                return response;
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//        }
//    }
//}
