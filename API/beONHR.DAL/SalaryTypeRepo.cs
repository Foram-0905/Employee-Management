using beONHR.Entities.DTO;
using beONHR.Entities.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface ISalaryTypeRepo
    {
        Task<ClientResponse> GetSalaryTypes();
    }

    public class SalaryTypeRepo : ISalaryTypeRepo
    {
        private readonly MainContext _context;

        public SalaryTypeRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> GetSalaryTypes()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var salaryTypes = await _context.SalaryTypes
                    .Where(x => x.IsDeleted != true)
                    .ToListAsync();

                if (salaryTypes == null || !salaryTypes.Any())
                {
                    response.Message = "No SalaryTypes found";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "SalaryTypes retrieved successfully";
                    response.HttpResponse = salaryTypes;
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
    }
}


