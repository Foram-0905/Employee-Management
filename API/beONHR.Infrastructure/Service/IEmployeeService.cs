using beONHR.DAL;
using beONHR.Entities;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface IEmployeeService
    {
        Task<ClientResponse> SaveEmployee(EmployeeDTO input); //, ICollection<LanguageCompetenceDTO> languageCompetences);
        Task<ClientResponse> GetEmployeeById(string id);
        Task<ClientResponse> GetEmployeeByLeader(string id);
        Task<ClientResponse> getEmployeeByHr(string id);
        Task<ClientResponse> GetAvailableLeaveByEmployeeId(Guid employeeId);
        Task<ClientResponse> GetEmployee();
        Task<ClientResponse> GetFilterEmployee(FilterRequsetDTO filterRequset);
        Task<ClientResponse> DeleteEmployee(Guid id);
    }
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo _employee;
        public EmployeeService(IEmployeeRepo employee)
        {
            _employee = employee;
        }

        public async Task<ClientResponse> SaveEmployee(EmployeeDTO input) // , ICollection<LanguageCompetenceDTO> languageCompetences)
        {
            try
            {
                // Call the SaveEmployee method of IEmployeeRepo with both EmployeeDTO and LanguageCompetenceDTO
                return await _employee.SaveEmployee(input); //, languageCompetences);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetAvailableLeaveByEmployeeId(Guid employeeId)
        {
            try
            {
                return await _employee.GetAvailableLeaveByEmployeeId(employeeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetEmployee()
        {
            try
            {
                return await _employee.GetEmployee();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetEmployeeById(string id)
        {
            try
            {
                return await _employee.GetEmployeeById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetEmployeeByLeader(string id)
        {
            try
            {
                return await _employee.GetEmployeeByLeader(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> getEmployeeByHr(string id)
        {
            try
            {
                return await _employee.getEmployeeByHr(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteEmployee(Guid id)
        {
            try
            {
                return await _employee.DeleteEmployee(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetFilterEmployee(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _employee.GetFilterEmployee(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
