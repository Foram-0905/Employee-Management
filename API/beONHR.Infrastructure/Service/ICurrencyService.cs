using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;


namespace beONHR.Infrastructure.Service
{
    public interface ICurrencyService
    {
        Task<ClientResponse> SaveCurrency(CurrencyDTO input);
        Task<ClientResponse> GetCurrency();

        Task<ClientResponse> GetFilterCurrency(FilterRequsetDTO filterRequset);
        Task<ClientResponse> DeleteCurrency(Guid id);

        Task<ClientResponse> GetCurrencyById(Guid id);
    }

    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepo _currencyRepo;

        public CurrencyService(ICurrencyRepo currencyRepo)
        {
            _currencyRepo = currencyRepo;
        }

        public async Task<ClientResponse> SaveCurrency(CurrencyDTO input)
        {
            try
            {
                return await _currencyRepo.SaveCurrency(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetCurrency()
        {
            try
            {
                return await _currencyRepo.GetCurrency();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetFilterCurrency(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _currencyRepo.GetFilterCurrency(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteCurrency(Guid id)
        {
            try
            {
                return await _currencyRepo.DeleteCurrency(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetCurrencyById(Guid id)
        {
            try
            {
                return await _currencyRepo.GetCurrencyById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
