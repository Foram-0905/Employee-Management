using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface IAssetsService
    {
        Task<ClientResponse> SaveAsset(AssetsDTO input);
        Task<ClientResponse> GetAssets();
        Task<ClientResponse> GetFilterAssets(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetAssetById(Guid id);
        Task<ClientResponse> GetAssetByEmployeeId(Guid id);
        Task<ClientResponse> DeleteAsset(Guid id);
    }

    public class AssetsService : IAssetsService
    {
        private readonly IAssetsRepo _assetsRepo;

        public AssetsService(IAssetsRepo assetsRepo)
        {
            _assetsRepo = assetsRepo;
        }

        public async Task<ClientResponse> SaveAsset(AssetsDTO input)
        {
            try
            {
                return await _assetsRepo.SaveAsset(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetAssets()
        {
            try
            {
                return await _assetsRepo.GetAssets();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetFilterAssets(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _assetsRepo.GetFilterAssets(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ClientResponse> GetAssetById(Guid id)
        {
            try
            {
                return await _assetsRepo.GetAssetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetAssetByEmployeeId(Guid id)
        {
            try
            {
                return await _assetsRepo.GetAssetByEmployeeId(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteAsset(Guid id)
        {
            try
            {
                return await _assetsRepo.DeleteAsset(id);
            }
            catch (Exception ex)
            {
                throw ex;     
            }
        }
    }
}
