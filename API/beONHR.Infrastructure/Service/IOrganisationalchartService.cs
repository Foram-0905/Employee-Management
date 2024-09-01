using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    public interface IOrganisationalchartService
    {
        Task<ClientResponse> GetEmployeeForChart();
    } 
    public class OrganisationalchartService: IOrganisationalchartService
    {
        private readonly IOrganisationalchartRepo _OrganisationalchartRepo;
        public OrganisationalchartService(IOrganisationalchartRepo OrganisationalchartRepo)
        {
            _OrganisationalchartRepo = OrganisationalchartRepo;
        }

        public async Task<ClientResponse> GetEmployeeForChart()
        {
            try
            {
                return await _OrganisationalchartRepo.GetEmployeeForChart();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
