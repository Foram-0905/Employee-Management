//using beONHR.DAL;
//using beONHR.Entities.DTO;
//using System;
//using System.Threading.Tasks;

//namespace beONHR.Infrastructure.Service
//{
//    public interface ILanguageCompetenceService
//    {
//        Task<ClientResponse> LanguageCompetenceGetByEmployeeIdAsync(Guid employeeId);

//        Task<ClientResponse> SaveLanguageCompetence(LanguageCompetenceDTO input);
//        Task<ClientResponse> GetLanguageCompetence();
//        Task<ClientResponse> DeleteLanguageCompetence(Guid Id);
//    }

//    public class LanguageCompetenceService : ILanguageCompetenceService
//    {
//        private readonly ILanguageCompetenceRepo _languageCompetenceRepo;

//        public LanguageCompetenceService(ILanguageCompetenceRepo languageCompetenceRepo)
//        {
//            _languageCompetenceRepo = languageCompetenceRepo;
//        }

//        public async Task<ClientResponse> SaveLanguageCompetence(LanguageCompetenceDTO input)
//        {
//            try
//            {
//                // Delegate the save operation to the repository
//                return await _languageCompetenceRepo.SaveLanguageCompetence(input);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<ClientResponse> GetLanguageCompetence()
//        {
//            try
//            {
//                // Delegate the get operation to the repository
//                return await _languageCompetenceRepo.GetLanguageCompetence();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<ClientResponse> DeleteLanguageCompetence(Guid id)
//        {
//            try
//            {
//                // Delegate the delete operation to the repository
//                return await _languageCompetenceRepo.DeleteLanguageCompetence(id);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        public async Task<ClientResponse> LanguageCompetenceGetByEmployeeIdAsync(Guid employeeId)
//        {
//            try
//            {
//                // Delegate the getByEmployeeIdAsync operation to the repository
//                return await _languageCompetenceRepo.LanguageCompetenceGetByEmployeeIdAsync(employeeId);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//    }
//}
