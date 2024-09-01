using beONHR.DAL;
using beONHR.Entities;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface ILeaveCategoryService
    {
        Task<ClientResponse> SaveLeaveCategory(LeaveCategoryDTO input);
        Task<ClientResponse> GetLeaveCategory();
        Task<ClientResponse> DeleteLeaveCategory(Guid LeaveCategoryId);
        Task<ClientResponse> GetLeaveCategoryById(Guid LeaveCategoryId);
        Task<ClientResponse> GetFilterLeaveCategory(FilterRequsetDTO filterRequset);

    }

    public class LeavecategoryService : ILeaveCategoryService
    {
        private readonly ILeaveCategoryRepo _leaveCategoryRepo;

        public LeavecategoryService(ILeaveCategoryRepo LeavecategoryRepo)
        {
            _leaveCategoryRepo = LeavecategoryRepo;
        }

        public async Task<ClientResponse> SaveLeaveCategory(LeaveCategoryDTO input)
        {
            try
            {
                return await _leaveCategoryRepo.SaveLeaveCategory(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetLeaveCategory()
        {
            try
            {
                return await _leaveCategoryRepo.GetLeaveCategory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetLeaveCategoryById(Guid id)
        {
            try
            {
                return await _leaveCategoryRepo.GetLeaveCategoryById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterLeaveCategory(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _leaveCategoryRepo.GetFilterLeaveCategory(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteLeaveCategory(Guid id)
        {
            try
            {
                return await _leaveCategoryRepo.DeleteLeaveCategory(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
