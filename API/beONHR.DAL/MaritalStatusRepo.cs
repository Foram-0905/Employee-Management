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
   

    public interface IMaritalStatusRepo
    {

        Task<ClientResponse> GetMaritalStatus();
    }
    public class MaritalStatusRepo : IMaritalStatusRepo
    {
        private readonly MainContext _mainContext;
        public MaritalStatusRepo(MainContext mainContext)
        {
            _mainContext = mainContext;
        }

        public async Task<ClientResponse>GetMaritalStatus()
        {
            ClientResponse response = new();
            try
            {
                // Get all assets
                var marital = await _mainContext.MaritalStatuses.Where(x => x.IsDeleted != true).OrderBy(x => x.maritalstatus).ToListAsync();

                response.Message = "MaritalStatus retrieved successfully";
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
