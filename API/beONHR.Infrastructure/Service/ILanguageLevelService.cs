using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    public interface ILanguageLevelService
    {
        Task<ClientResponse> DeleteLanguageLevel(Guid id);
        Task<ClientResponse> GetLanguageLevelById(Guid id);
        Task<ClientResponse> GetLanguageLevel();
        Task<ClientResponse> SaveLanguageLevel(LanguageLevelDTO input);
        Task<ClientResponse> GetFilterLanguageLevel(FilterRequsetDTO filterRequset);
       
    }
    public class LanguageLevelService : ILanguageLevelService
    {
        private readonly ILanguageLevelRepo _languageLevelRepo;

        public LanguageLevelService(ILanguageLevelRepo languageLevelRepo)
        {
            _languageLevelRepo = languageLevelRepo;
        }

        public async Task<ClientResponse> SaveLanguageLevel(LanguageLevelDTO input)
        {
            try
            {
                return await _languageLevelRepo.SaveLanguageLevel(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetLanguageLevel()
        {
            try
            {
                return await _languageLevelRepo.GetLanguageLevel();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteLanguageLevel(Guid id)
        {
            try
            {
                return await _languageLevelRepo.DeleteLanguageLevel(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetLanguageLevelById(Guid id)
        {
            try
            {
                return await _languageLevelRepo.GetLanguageLevelById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterLanguageLevel(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _languageLevelRepo.GetFilterLanguageLevel(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}