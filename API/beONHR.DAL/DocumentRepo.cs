using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface IDocumentRepo
    {
        Task<ClientResponse> SaveDocument(DocumentDTO input);
        Task<ClientResponse> GetDocument();
        Task<ClientResponse> DeleteDocument(Guid id);
        Task<ClientResponse> GetDocumentById(Guid id);
    }
    public class DocumentRepo : IDocumentRepo
    {
        private readonly MainContext _context;
        public DocumentRepo(MainContext context)
        {
            _context = context;
        }
        public async Task<ClientResponse> SaveDocument(DocumentDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var designation = await _context.Documents.Where(x => x.PersonalDataProtection == input.PersonalDataProtection && x.EmployeeId == input.EmployeeId && x.IsDeleted != true).FirstOrDefaultAsync();
                    //3fa85f64 - 5717 - 4562 - b3fc - 2c963f66afa6
                    if (designation == null)
                    {
                        Document model = new Document
                        {
                            PersonalDataProtection = input.PersonalDataProtection,
                            ConfidentialityAgreement = input.ConfidentialityAgreement,
                            EmployeeId = input.EmployeeId,
                            Other = input.Other,
                        };

                        await _context.Documents.AddAsync(model);
                        var res = await _context.SaveChangesAsync();


                        if (res == 0)
                        {
                            response.Message = "Documents not insert";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Documents insert Sucesfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Documents already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }
                    return response;
                }
                else
                {
                    var document = await _context.Documents.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefaultAsync();

                    if (document != null)
                    {
                        document.Id = input.Id;
                        document.PersonalDataProtection = input.PersonalDataProtection;
                        document.ConfidentialityAgreement = input.ConfidentialityAgreement;
                        document.EmployeeId = input.EmployeeId;
                        document.Other = input.Other;


                        _context.Documents.Update(document);
                        var res = await _context.SaveChangesAsync();


                        if (res == 0)
                        {
                            response.Message = "Documents not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Documents updated Sucesfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Documents Not Exists";
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

        public async Task<ClientResponse> GetDocument()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var document = await _context.Documents.Include(x => x.Employee).Where(x => x.IsDeleted != true).ToListAsync();

                if (document == null)
                {
                    response.Message = "No Any Documents";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                response.Message = "Documents Get Sucesfully";
                response.HttpResponse = document;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ClientResponse> DeleteDocument(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var document = await _context.Documents.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (document != null)
                {
                    document.IsDeleted = true;
                    _context.Documents.Update(document);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "Documents Deleted Faild";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Documents Deleted Sucesfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Documents not Exists";
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
        public async Task<ClientResponse> GetDocumentById(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var document = await _context.Documents.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (document != null)
                {

                    response.Message = "Documents Get Sucesfully";
                    response.HttpResponse = document;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "Documents not Exists";
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
