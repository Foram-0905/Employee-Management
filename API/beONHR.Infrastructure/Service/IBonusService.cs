using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;


namespace beONHR.Infrastructure.Service
{
    public interface IBonusService
    {
        Task<ClientResponse> SaveBonus(BonusDTO input);
        Task<ClientResponse> GetBonus();

        Task<ClientResponse> GetFilterBonus(FilterRequsetDTO filterRequset);
        Task<ClientResponse> DeleteBonus(Guid id);

        Task<ClientResponse> GetBonusById(Guid id);
    }

    public class BonusService : IBonusService
    {
        private readonly IBonusRepo _bonusRepo;

        public BonusService(IBonusRepo bonusRepo)
        {
            _bonusRepo = bonusRepo;
        }

        public async Task<ClientResponse> SaveBonus(BonusDTO input)
        {
            try
            {
                return await _bonusRepo.SaveBonus(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetBonus()
        {
            try
            {
                return await _bonusRepo.GetBonus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetFilterBonus(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _bonusRepo.GetFilterBonus(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteBonus(Guid id)
        {
            try
            {
                return await _bonusRepo.DeleteBonus(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetBonusById(Guid id)
        {
            try
            {
                return await _bonusRepo.GetBonusById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}