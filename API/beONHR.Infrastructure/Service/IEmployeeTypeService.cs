using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface IEmployeeTypeService
    {
        
        Task<ClientResponse> GetEmployeeType();
      
    }

    public class EmployeeTypeService : IEmployeeTypeService
    {
        private readonly IEmployeetypeRepo _employeeTypeRepo;

        public EmployeeTypeService( IEmployeetypeRepo employeeTypeRepo)
        {
            _employeeTypeRepo = employeeTypeRepo;
        }

  

        public async Task<ClientResponse> GetEmployeeType()
        {
            try
            {
                return await _employeeTypeRepo.GetEmplyoeetype();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
     

    
    }
}
