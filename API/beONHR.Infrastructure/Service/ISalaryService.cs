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
    public interface ISalaryService
    {
        Task<ClientResponse> SaveSalary(SalaryDTO input);
        Task<ClientResponse> GetSalary();
        Task<ClientResponse> DeleteSalary(Guid id);
        Task<ClientResponse> GetPreviousMonthSalary(Guid id);
        Task<ClientResponse> GetTwoMonthsAgoSalary(Guid id);
        Task<ClientResponse> GetSalaryById(Guid id);
        Task<ClientResponse> GetFilterSalary(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetSalaryByEmployee(Guid id);
    }
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepo _salary;
        public SalaryService(ISalaryRepo salary)
        {
            _salary = salary;
        }
        public async Task<ClientResponse> SaveSalary(SalaryDTO input)
        {
            try
            {
                return await _salary.SaveSalary(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetSalary()
        {
            try
            {
                return await _salary.GetSalary();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteSalary(Guid id)
        {
            try
            {
                return await _salary.DeleteSalary(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetPreviousMonthSalary(Guid id)
        {
            try
            {
                return await _salary.GetPreviousMonthSalary(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetTwoMonthsAgoSalary(Guid id)
        {
            try
            {
                return await _salary.GetTwoMonthsAgoSalary(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetFilterSalary(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _salary.GetFilterSalary(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetSalaryById(Guid id)
        {
            try
            {
                return await _salary.GetSalaryById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetSalaryByEmployee(Guid id)
        {
            try
            {
                return await _salary.GetSalaryByEmployee(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
