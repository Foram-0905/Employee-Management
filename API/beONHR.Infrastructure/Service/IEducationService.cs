using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface IEducationService
    {
        Task<ClientResponse> SaveEducation(EducationDTO input);
        Task<ClientResponse> GetEducation();
        Task<ClientResponse> GetFilterEducation(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetEducationById(Guid id);
        Task<ClientResponse> DeleteEducation(Guid id);
        Task<ClientResponse> GetEducationByEmployee(Guid employeeId); // Add this line

    }

    public class EducationService : IEducationService
    {
        private readonly IEducationRepo _educationRepo;

        public EducationService(IEducationRepo educationRepo)
        {
            _educationRepo = educationRepo;
        }

        public async Task<ClientResponse> SaveEducation(EducationDTO input)
        {
            try
            {
                return await _educationRepo.SaveEducation(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetEducation()
        {
            try
            {
                return await _educationRepo.GetEducation();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetFilterEducation(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _educationRepo.GetFilterEducation(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ClientResponse> GetEducationById(Guid id)
        {
            try
            {
                return await _educationRepo.GetEducationById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetEducationByEmployee(Guid employeeId)
        {
            try
            {
                return await _educationRepo.GetEducationByEmployee(employeeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteEducation(Guid id)
        {
            try
            {
                return await _educationRepo.DeleteEducation(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
