using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
      public interface IJobHistoryService
    {
        Task<ClientResponse> SaveJobHistory(JobHistoryDTO input);
        Task<ClientResponse> GetFilterJobHistory(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetJobHistory();
        Task<ClientResponse> DeleteJobHistory(Guid id);
        Task<ClientResponse> GetJobHistoryById(Guid id);
        Task<ClientResponse> GetJobHistoryByEmployeeId(Guid id);
    }
    public class JobHistoryService : IJobHistoryService
    {
        private readonly IJobHistoryRepo _jobHistoryRepo;

        public JobHistoryService(IJobHistoryRepo jobHistoryRepo)
        {
            _jobHistoryRepo = jobHistoryRepo;
        }

        public async Task<ClientResponse> SaveJobHistory(JobHistoryDTO input)
        {
            try
            {
                return await _jobHistoryRepo.SaveJobHistory(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetJobHistory()
        {
            try
            {
                return await _jobHistoryRepo.GetJobHistory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterJobHistory(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _jobHistoryRepo.GetFilterJobHistory(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetJobHistoryById(Guid id)
        {
            try
            {
                return await _jobHistoryRepo.GetJobHistoryById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetJobHistoryByEmployeeId(Guid id)
        {
            try
            {
                return await _jobHistoryRepo.GetJobHistoryByEmployeeId(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteJobHistory(Guid id)
        {
            try
            {
                return await _jobHistoryRepo.DeleteJobHistory(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
