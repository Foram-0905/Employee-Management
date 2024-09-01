using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface IEductionLevelService
    {
        Task<ClientResponse> DeleteEductionLevel(Guid id);
        Task<ClientResponse> GetEductionLevelById(Guid id);
        Task<ClientResponse> GetFilterEductionLevel(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetEductionLevel();
        Task<ClientResponse> SaveEductionLevel(EductionLevelDTO input);
    }
    public class EductionLevelService : IEductionLevelService
    {
        private readonly IEductionLevelRepo _eductionLevelRepo;

        public EductionLevelService(IEductionLevelRepo eductionLevelRepo)
        {
            _eductionLevelRepo = eductionLevelRepo;
        }

        public async Task<ClientResponse> SaveEductionLevel(EductionLevelDTO input)
        {
            try
            {
                return await _eductionLevelRepo.SaveEductionLevel(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetEductionLevel()
        {
            try
            {
                return await _eductionLevelRepo.GetEductionLevel();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteEductionLevel(Guid id)
        {
            try
            {
                return await _eductionLevelRepo.DeleteEductionLevel(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetEductionLevelById(Guid id)
        {
            try
            {
                return await _eductionLevelRepo.GetEductionLevelById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterEductionLevel(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _eductionLevelRepo.GetFilterEductionLevel(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}