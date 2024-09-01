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
    public interface IEmploymentTypeRepo
    {
        Task<ClientResponse> GetEmploymentType();
    }
    public class EmploymentTypeRepo : IEmploymentTypeRepo
    {
        private readonly MainContext _mainContext;
        public EmploymentTypeRepo(MainContext mainContext)
        {
            _mainContext = mainContext;
        }

        public async Task<ClientResponse> GetEmploymentType()
        {
            ClientResponse response = new();
            try
            {
                // Get all assets
                var marital = await _mainContext.EmploymentTypes.Where(x => x.IsDeleted != true).ToListAsync();

                response.Message = "EmploymentTypes retrieved successfully";
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
