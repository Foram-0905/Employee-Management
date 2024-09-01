using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface IAssets_StatusService
    {

        Task<ClientResponse> GetAssets_Status();

    }

    public class Assets_StatusService : IAssets_StatusService
    {
        private readonly IAssets_StatusRepo _assets_statusRepo;

        public Assets_StatusService(IAssets_StatusRepo assets_statusRepo)
        {
            _assets_statusRepo = assets_statusRepo;
        }



        public async Task<ClientResponse> GetAssets_Status()
        {
            try
            {
                return await _assets_statusRepo.GetAssets_Status();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

 
    }
}
