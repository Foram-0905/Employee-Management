using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    public interface  IEmploymentTypeService
    {
        Task<ClientResponse> GetEmploymentType();
    }
    public class EmploymentTypeService : IEmploymentTypeService
    {
        private readonly IEmploymentTypeRepo _employmenttypeRepo;
        public EmploymentTypeService(IEmploymentTypeRepo maritalrepo)
        {
            _employmenttypeRepo = maritalrepo;
        }

        public async Task<ClientResponse> GetEmploymentType()
        {
            try
            {
                return await _employmenttypeRepo.GetEmploymentType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
