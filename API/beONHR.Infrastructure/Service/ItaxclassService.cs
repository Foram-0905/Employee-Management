using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface ItaxclassService
    {

        Task<ClientResponse> Gettaxclass();

    }

    public class taxclassService : ItaxclassService
    {
        private readonly ItaxclassRepo _taxclassRepo;

        public taxclassService(ItaxclassRepo taxclassRepo)
        {
            _taxclassRepo = taxclassRepo;
        }



        public async Task<ClientResponse> Gettaxclass()
        {
            try
            {
                return await _taxclassRepo.Gettaxclass();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
