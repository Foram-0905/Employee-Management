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
    public interface IDeliverymethodRepo
    {
        Task<ClientResponse> GetDeliverymethod();
    }
    public class DeliverymethodRepo : IDeliverymethodRepo
    {
        private readonly MainContext _mainContext;
        public DeliverymethodRepo(MainContext mainContext)
        {
            _mainContext = mainContext;
        }

        public async Task<ClientResponse> GetDeliverymethod()
        {
            ClientResponse response = new();
            try
            {
                // Get all assets
                var marital = await _mainContext.Deliverymethods.Where(x => x.IsDeleted != true).OrderBy(x => x.deliveryMethod).ToListAsync();

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
