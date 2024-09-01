using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface IEmployeenStatusRepo
    {
        Task<ClientResponse> GetEmployeenStatus();
    }
    public class EmployeenStatusRepo : IEmployeenStatusRepo
    {
        private readonly MainContext _mainContext;
        public EmployeenStatusRepo(MainContext mainContext)
        {
            _mainContext = mainContext;
        }

        public async Task<ClientResponse> GetEmployeenStatus()
        {
            ClientResponse response = new();
            try
            {
                // Get all assets
                var marital = await _mainContext.EmployeenStatuses.Where(x => x.IsDeleted != true).OrderBy(x => x.employeenstatus).ToListAsync();

                response.Message = "EmployeenStatus retrieved successfully";
                response.HttpResponse = marital;
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
