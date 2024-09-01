using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{

    public interface IMaritalStatusService
    {

        Task<ClientResponse> GetMaritalStatus();

    }
    public class MaritalStatusService : IMaritalStatusService
    {
        private readonly IMaritalStatusRepo _maritalrepo;
        public MaritalStatusService(IMaritalStatusRepo maritalrepo)
        {
            _maritalrepo = maritalrepo;
        }

        public async Task<ClientResponse> GetMaritalStatus()
        {
            try
            {
                return await _maritalrepo.GetMaritalStatus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

  

}



