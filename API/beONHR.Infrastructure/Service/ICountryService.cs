using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface ICountryService
    {
        Task<ClientResponse> SaveCountry(CountryDTO input);
        Task<ClientResponse> GetCountry();
        Task<ClientResponse> DeleteCountry(Guid id);
        Task<ClientResponse> GetCountryById(Guid Id);
        Task<ClientResponse> GetFilterCountry(FilterRequsetDTO filterRequset);

    }

    public class CountryService : ICountryService
    {
        private readonly ICountryRepo _countryRepo;

        public CountryService(ICountryRepo countryRepo)
        {
            _countryRepo = countryRepo;
        }

        public async Task<ClientResponse> SaveCountry(CountryDTO input)
        {
            try
            {
                return await _countryRepo.SaveCountry(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetCountry()
        {
            try
            {
                return await _countryRepo.GetCountry();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetCountryById(Guid Id)
        {
            try
            {
                return await _countryRepo.GetCountryById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterCountry(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _countryRepo.GetFilterCountry(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteCountry(Guid id)
        {
            try
            {
                return await _countryRepo.DeleteCountry(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
