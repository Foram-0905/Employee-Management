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
    //public interface IEmployeeChildrenRepo
    //{
    //    Task<ClientResponse> SaveEmployeeChildren(EmployeeChildrenDTO input);
    //    Task<ClientResponse> DeleteEmployeeChildren(Guid id);
    //    Task<ClientResponse> GetEmployeeChildren();
    //}
    //public class EmployeeChildrenRepo : IEmployeeChildrenRepo
    //{
    //    private readonly MainContext _mainContext;
    //    public EmployeeChildrenRepo(MainContext mainContext) 
    //    { 
    //        _mainContext = mainContext;
    //    }

       
    //    public async Task<ClientResponse> SaveEmployeeChildren(EmployeeChildrenDTO input)
    //    {
    //        ClientResponse response = new();
    //        try
    //        {
    //            if (input.Action == ActionEnum.Insert)
    //            {
    //                var employeechildren = await _mainContext.EmployeeChildrens.Where(x =>
    //                x.FirstName==input.FirstName && 
    //                x.FamilyName==input.FamilyName &&
    //                x.BirthDate==input.BirthDate &&
    //                x.Locationchildregistered==input.Locationchildregistered &&
    //                x.socialcareinsurance==input.socialcareinsurance &&
    //                x.BirthCertificate==input.BirthCertificate &&
    //                x.EmployeeId==input.EmployeeId).FirstOrDefaultAsync();

    //                if (employeechildren == null)
    //                {
    //                    EmployeeChildren model = new EmployeeChildren
    //                    {
    //                        FirstName=input.FirstName,
    //                        FamilyName=input.FamilyName,
    //                        BirthDate=input.BirthDate,
    //                        Locationchildregistered=input.Locationchildregistered ,
    //                        socialcareinsurance=input.socialcareinsurance ,
    //                        BirthCertificate=input.BirthCertificate,
    //                        EmployeeId=input.EmployeeId

    //                    };

    //                    await _mainContext.EmployeeChildrens.AddAsync(model);
    //                    var res = await _mainContext.SaveChangesAsync();

    //                    if (res == 0)
    //                    {
    //                        response.Message = "Slggroup not saved";
    //                        response.StatusCode = HttpStatusCode.NoContent;
    //                        response.IsSuccess = false;
    //                    }
    //                    else
    //                    {
    //                        response.Message = "Slggroup saved successfully";
    //                        response.HttpResponse = model.Id;
    //                        response.IsSuccess = true;
    //                        response.StatusCode = HttpStatusCode.OK;
    //                    }
    //                }
    //                else
    //                {
    //                    response.Message = "Slggroup already exists";
    //                    response.StatusCode = HttpStatusCode.BadRequest;
    //                    response.IsSuccess = false;
    //                }

    //                return response;
    //            }
    //            else
    //            {
    //                var employeechildren = await _mainContext.EmployeeChildrens.Where(x => x.Id == input.Id && x.IsDeleted != true).FirstOrDefaultAsync();
    //                if (employeechildren != null)
    //                {
    //                    employeechildren.FirstName = input.FirstName;
    //                    employeechildren.FamilyName = input.FamilyName;
    //                    employeechildren.BirthDate = input.BirthDate;
    //                    employeechildren.Locationchildregistered = input.Locationchildregistered;
    //                    employeechildren.socialcareinsurance = input.socialcareinsurance;
    //                    employeechildren.BirthCertificate = input.BirthCertificate;
    //                   employeechildren. EmployeeId = input.EmployeeId;
    //                    _mainContext.EmployeeChildrens.Update(employeechildren);
    //                    var res = await _mainContext.SaveChangesAsync();

    //                    if (res == 0)
    //                    {
    //                        response.Message = "employeechildren not updated";
    //                        response.StatusCode = HttpStatusCode.NoContent;
    //                        response.IsSuccess = false;
    //                    }
    //                    else
    //                    {
    //                        response.Message = "employeechildren updated Sucesfully";
    //                        response.HttpResponse = null;
    //                        response.IsSuccess = true;
    //                        response.StatusCode = HttpStatusCode.OK;
    //                    }
    //                }
    //                else
    //                {
    //                    response.Message = "employeechildren Not Found";
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
    //    public async Task<ClientResponse> DeleteEmployeeChildren(Guid id)
    //    {

    //        ClientResponse response = new();
    //        try
    //        {

    //            var employeechildren = await _mainContext.EmployeeChildrens.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

    //            if (employeechildren != null)
    //            {
    //                employeechildren.IsDeleted = true;


    //                _mainContext.EmployeeChildrens.Update(employeechildren);
    //                var res = await _mainContext.SaveChangesAsync();


    //                if (res == 0)
    //                {
    //                    response.Message = "EmployeeChildrens Deleted Faild";
    //                    response.StatusCode = HttpStatusCode.NoContent;
    //                    response.IsSuccess = false;
    //                }
    //                else
    //                {
    //                    response.Message = "EmployeeChildrens Deleted Sucesfully";
    //                    response.HttpResponse = null;
    //                    response.IsSuccess = true;
    //                    response.StatusCode = HttpStatusCode.OK;
    //                }
    //            }
    //            else
    //            {
    //                response.Message = "EmployeeChildrens not Exists";
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
    //    public async Task<ClientResponse> GetEmployeeChildren()
    //    {
    //        ClientResponse response = new ClientResponse();
    //        try
    //        {
    //            var employeechildren = await _mainContext.EmployeeChildrens.Where(x => x.IsDeleted != true).Include(x => x.Employee).ToListAsync();

    //            if (employeechildren == null)
    //            {
    //                response.Message = "No EmployeeChildrens found";
    //                response.StatusCode = HttpStatusCode.NoContent;
    //                response.IsSuccess = false;
    //            }
    //            else
    //            {
    //                response.Message = "EmployeeChildrens retrieved successfully";
    //                response.HttpResponse = employeechildren;
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
