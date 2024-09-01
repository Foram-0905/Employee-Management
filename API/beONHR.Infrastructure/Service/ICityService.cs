using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface ICityService
    {
        Task<ClientResponse> SaveCity(CityDto input);
        Task<ClientResponse> GetCity();
        Task<ClientResponse> DeleteCity(Guid cityId);
        Task<ClientResponse> GetFilterCity(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetCityByState(Guid stateId);

        Task<ClientResponse> GetCityById(Guid cityId);
    }

    public class CityService : ICityService
    {
        private readonly ICityRepo _cityRepo;

        public CityService(ICityRepo cityRepo)
        {
            _cityRepo = cityRepo;
        }

        public async Task<ClientResponse> SaveCity(CityDto input)
        {
            try
            {
                return await _cityRepo.SaveCity(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetCity()
        {
            try
            {
                return await _cityRepo.GetCities();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterCity(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _cityRepo.GetFilterCity(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetCityById(Guid id)
        {
            try
            {
                return await _cityRepo.GetCityById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetCityByState(Guid stateId)
        {
            try
            {
                return await _cityRepo.GetCityByState(stateId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteCity(Guid id)
        {
            try
            {
                return await _cityRepo.DeleteCity(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
