using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface IAssets_typeService
    {

        Task<ClientResponse> GetAssets_type();

    }

    public class Assets_typeService : IAssets_typeService
    {
        private readonly IAssets_typeRepo _assets_typeRepo;

        public Assets_typeService(IAssets_typeRepo assets_typeRepo)
        {
            _assets_typeRepo = assets_typeRepo;
        }



        public async Task<ClientResponse> GetAssets_type()
        {
            try
            {
                return await _assets_typeRepo.GetAssets_type();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
