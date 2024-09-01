using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface ILeaveTypeEmployeeRepo
    {
        Task<ClientResponse> GetLeaveTypeEmployee();
    }
    public class LeaveTypeEmployeeRepo : ILeaveTypeEmployeeRepo
    {
        private readonly MainContext _context;

        public LeaveTypeEmployeeRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> GetLeaveTypeEmployee()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var EmployeeType = await _context.LeaveTypeEmployees.Where(x => !x.IsDeleted).ToListAsync();

                if (EmployeeType != null && EmployeeType.Count > 0)
                {
                    response.Message = "LeaveTypeEmployee retrieved successfully";
                    response.HttpResponse = EmployeeType;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "No LeaveTypeEmployee found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
