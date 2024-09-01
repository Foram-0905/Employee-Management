using beONHR.DAL;
using beONHR.Entities;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface ISlggroupService
    {
        Task<ClientResponse> SaveSlggroup(SlggroupDto input);

        Task<ClientResponse> GetFilterSlggroup(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetSlggroup();

        Task<ClientResponse> GetSLGgroupById(Guid id);

        Task<ClientResponse> DeleteSlggroup(Guid id);
       
    }

    public class SlggroupService : ISlggroupService
    {
        private readonly ISlggroupRepo _slggroupRepo;

        public SlggroupService(ISlggroupRepo slggroupRepo)
        {
            _slggroupRepo = slggroupRepo;
        }

        public async Task<ClientResponse> SaveSlggroup(SlggroupDto input)
        {
            try
            {
                return await _slggroupRepo.SaveSlggroup(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetSlggroup()
        {
            try
            {
                return await _slggroupRepo.GetSlggroup();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteSlggroup(Guid id)
        {
            try
            {
                return await _slggroupRepo.DeleteSlggroup(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetSLGgroupById(Guid id)
        {
            try
            {
                return await _slggroupRepo.GetSLGgroupById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterSlggroup(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _slggroupRepo.GetFilterSlggroup(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
