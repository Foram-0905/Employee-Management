using beONHR.Entities.DTO;
using beONHR.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface IStateService
    {
        Task<ClientResponse> SaveState(StateDto input);

        Task<ClientResponse> GetState();
        Task<ClientResponse> GetFilterState(FilterRequsetDTO filterRequset);
        Task<ClientResponse> DeleteState(Guid id);
        Task<ClientResponse> GetStateById(Guid id);
        Task<ClientResponse> GetStatesByCountryId(Guid countryId);

    }

    public class StateService : IStateService
    {
        private readonly IStateRepo _IStateRepo;

        public StateService(IStateRepo IStateRepo)
        {
            _IStateRepo = IStateRepo;
        }

        public async Task<ClientResponse> SaveState(StateDto input)
        {
            try
            {
                return await _IStateRepo.SaveState(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetStatesByCountryId(Guid countryId)
        {
            try
            {
                return await _IStateRepo.GetStatesByCountryId(countryId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetState()
        {
            try
            {
                return await _IStateRepo.GetState();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteState(Guid id)
        {
            try
            {
                return await _IStateRepo.DeleteState(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetStateById(Guid id)
        {
            try
            {
                return await _IStateRepo.GetStateById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterState(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _IStateRepo.GetFilterState(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
