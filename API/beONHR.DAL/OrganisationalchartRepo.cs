using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface IOrganisationalchartRepo
    {
        Task<ClientResponse> GetEmployeeForChart();

    }
    public class OrganisationalchartRepo:IOrganisationalchartRepo
    {
        private readonly MainContext _context;
        public OrganisationalchartRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> GetEmployeeForChart()
        {
            ClientResponse response = new();

            try
            {
                var Employee = await _context.Employees.Where(x => x.IsDeleted == false).Include(x => x.SLGGroup).Include(x => x.ManageDesignation).Include(x => x.EmploymentType).Select(x => new OrganisationalchartDTO
                {
                    EmployeeName = x.FirstName + " " + x.MiddleName + " " + x.LastName,
                    EmployeementType = x.EmploymentType.employmenttype,
                    SlgName = x.SLGGroup.InitialStatus,
                    Designation = x.ManageDesignation.Designation
                }).GroupBy(p => p.SlgName).Select(g => new GroupedRecordViewModel
                {
                    Group=g.Key,
                    Names=g.ToList()
                }).ToListAsync();

                if(Employee != null)
                {
                    response.Message = "Employee retrieved successfully";
                    response.HttpResponse = Employee;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
