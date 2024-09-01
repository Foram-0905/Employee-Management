using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface IEmployeetypeRepo
    {
 
        Task<ClientResponse> GetEmplyoeetype();
    }

    public class EmployeeTypeRepo : IEmployeetypeRepo
    {
        private readonly MainContext _context;

        public EmployeeTypeRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> GetEmplyoeetype()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var EmployeeType = await _context.EmployeeTypes.Where(x => !x.IsDeleted ).ToListAsync();

                if (EmployeeType != null && EmployeeType.Count > 0)
                {
                    response.Message = "EmployeeType retrieved successfully";
                    response.HttpResponse = EmployeeType;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "No EmployeeType found";
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
