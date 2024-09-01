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
    public interface IConsultantRateService
    {
        Task<ClientResponse> SaveConsultantRate(ConsultantRateDTO input);

    }

    public class ConsultantRateService : IConsultantRateService
    {
        private readonly IConsultantRateRepo _consultant;
        public ConsultantRateService(IConsultantRateRepo consultant)
        {
            _consultant = consultant;
        }
        public async Task<ClientResponse> SaveConsultantRate(ConsultantRateDTO input)
        {
            try
            {
                return await _consultant.SaveConsultantRate(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
