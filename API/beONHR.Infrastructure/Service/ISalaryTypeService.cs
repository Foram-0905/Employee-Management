using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    public interface ISalaryTypeService
    {
        Task<ClientResponse> GetSalaryTypes();
    }

    public class SalaryTypeService : ISalaryTypeService
    {
        private readonly ISalaryTypeRepo _salaryTypeRepo;

        public SalaryTypeService(ISalaryTypeRepo salaryTypeRepo)
        {
            _salaryTypeRepo = salaryTypeRepo;
        }

        public async Task<ClientResponse> GetSalaryTypes()
        {
            try
            {
                return await _salaryTypeRepo.GetSalaryTypes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}