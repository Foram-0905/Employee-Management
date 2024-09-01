using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    public interface IDeliverymethodService
    {
        Task<ClientResponse> GetDeliverymethod();
    }
    public class DeliverymethodService : IDeliverymethodService
    {
        private readonly IDeliverymethodRepo _deliverymethodRepo;
        public DeliverymethodService(IDeliverymethodRepo deliverymethodRepo)
        {
            _deliverymethodRepo = deliverymethodRepo;
        }

        public async Task<ClientResponse> GetDeliverymethod()
        {
            try
            {
                return await _deliverymethodRepo.GetDeliverymethod();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
