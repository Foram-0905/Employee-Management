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
    //public interface IProbationRepo
    //{
    //    Task<ClientResponse> SaveProbation(ProbationDTO input);
    //    Task<ClientResponse> DeleteProbation(Guid id);
    //    Task<ClientResponse> GetProbation();
    //}
    //public class ProbationRepo : IProbationRepo
    //{
    //    private readonly MainContext _mainContext;

    //    public ProbationRepo (MainContext mainContext)
    //    {
    //        _mainContext = mainContext;
    //    }

       

    //    public async Task<ClientResponse> SaveProbation(ProbationDTO input)
    //    {
    //        ClientResponse response = new();
    //        try
    //        {
    //            if (input.Action == ActionEnum.Insert)
    //            {
    //                var probations = await _mainContext.Probations.Where(x => x.StartDate==input.StartDate && x.EndDate==input.EndDate && x.AdjustedEndDate==input.AdjustedEndDate && x.AdjustedDocument==input.AdjustedDocument && x.EmployeeId==input.EmployeeId).FirstOrDefaultAsync();

    //                if (probations == null)
    //                {
    //                    Probation model = new Probation
    //                    {
    //                         StartDate=input.StartDate,  
    //                         EndDate=input.EndDate,
    //                         AdjustedEndDate=input.AdjustedEndDate, 
    //                         AdjustedDocument=input.AdjustedDocument,  
    //                         EmployeeId=input.EmployeeId

    //                    };

    //                    await _mainContext.Probations.AddAsync(model);
    //                    var res = await _mainContext.SaveChangesAsync();

    //                    if (res == 0)
    //                    {
    //                        response.Message = "Probation not saved";
    //                        response.StatusCode = HttpStatusCode.NoContent;
    //                        response.IsSuccess = false;
    //                    }
    //                    else
    //                    {
    //                        response.Message = "Probation saved successfully";
    //                        response.HttpResponse = model.Id;
    //                        response.IsSuccess = true;
    //                        response.StatusCode = HttpStatusCode.OK;
    //                    }
    //                }
    //                else
    //                {
    //                    response.Message = "Probation already exists";
    //                    response.StatusCode = HttpStatusCode.BadRequest;
    //                    response.IsSuccess = false;
    //                }

    //                return response;
    //            }
    //            else
    //            {
    //                var probations = await _mainContext.Probations.Where(x => x.Id == input.Id && x.IsDeleted != true).FirstOrDefaultAsync();
    //                if (probations != null)
    //                {
    //                    probations.StartDate = input.StartDate;
    //                    probations.EndDate = input.EndDate;
    //                    probations.AdjustedEndDate = input.AdjustedEndDate;
    //                    probations.AdjustedDocument = input.AdjustedDocument;
    //                    probations.EmployeeId = input.EmployeeId;

    //                    _mainContext.Probations.Update(probations);
    //                    var res = await _mainContext.SaveChangesAsync();

    //                    if (res == 0)
    //                    {
    //                        response.Message = "Probation not updated";
    //                        response.StatusCode = HttpStatusCode.NoContent;
    //                        response.IsSuccess = false;
    //                    }
    //                    else
    //                    {
    //                        response.Message = "Probation updated Sucesfully";
    //                        response.HttpResponse = null;
    //                        response.IsSuccess = true;
    //                        response.StatusCode = HttpStatusCode.OK;
    //                    }
    //                }
    //                else
    //                {
    //                    response.Message = "Probation Not Found";
    //                    response.StatusCode = HttpStatusCode.NoContent;
    //                    response.IsSuccess = false;
    //                }
    //                return response;
    //            }


    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //    }
    //    public async Task<ClientResponse> DeleteProbation(Guid id)
    //    {

    //        ClientResponse response = new();
    //        try
    //        {

    //            var probations = await _mainContext.Probations.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

    //            if (probations != null)
    //            {
    //                probations.IsDeleted = true;


    //                _mainContext.Probations.Update(probations);
    //                var res = await _mainContext.SaveChangesAsync();


    //                if (res == 0)
    //                {
    //                    response.Message = "Probations Deleted Faild";
    //                    response.StatusCode = HttpStatusCode.NoContent;
    //                    response.IsSuccess = false;
    //                }
    //                else
    //                {
    //                    response.Message = "Probations Deleted Sucesfully";
    //                    response.HttpResponse = null;
    //                    response.IsSuccess = true;
    //                    response.StatusCode = HttpStatusCode.OK;
    //                }
    //            }
    //            else
    //            {
    //                response.Message = "Probations not Exists";
    //                response.StatusCode = HttpStatusCode.NoContent;
    //                response.IsSuccess = false;
    //            }

    //            return response;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }

    //    public async Task<ClientResponse> GetProbation()
    //    {
    //        ClientResponse response = new ClientResponse();
    //        try
    //        {
    //            var probations = await _mainContext.Probations.Where(x => x.IsDeleted != true).Include(x=>x.Employee).ToListAsync();

    //            if (probations == null)
    //            {
    //                response.Message = "No Probations found";
    //                response.StatusCode = HttpStatusCode.NoContent;
    //                response.IsSuccess = false;
    //            }
    //            else
    //            {
    //                response.Message = "Probations retrieved successfully";
    //                response.HttpResponse = probations;
    //                response.IsSuccess = true;
    //                response.StatusCode = HttpStatusCode.OK;
    //            }

    //            return response;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }
    //}
}
