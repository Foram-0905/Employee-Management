using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    public interface IEmployeenStatusService
    {
        Task<ClientResponse> GetEmployeenStatus();
    }
    public class EmployeenStatusService : IEmployeenStatusService
    {
        private readonly IEmployeenStatusRepo _employmenttypeRepo;
        public EmployeenStatusService(IEmployeenStatusRepo maritalrepo)
        {
            _employmenttypeRepo = maritalrepo;
        }

        public async Task<ClientResponse> GetEmployeenStatus()
        {
            try
            {
                return await _employmenttypeRepo.GetEmployeenStatus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
