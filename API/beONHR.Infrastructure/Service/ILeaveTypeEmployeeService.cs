using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    public interface ILeaveTypeEmployeeService
    {
        Task<ClientResponse> GetLeaveTypeEmployee();
    }

    public class LeaveTypeEmployeeService : ILeaveTypeEmployeeService
    {
        private readonly ILeaveTypeEmployeeRepo _leavetypeEmployeeRepo;

        public LeaveTypeEmployeeService(ILeaveTypeEmployeeRepo leavetypeEmployeeRepo)
        {
            _leavetypeEmployeeRepo = leavetypeEmployeeRepo;
        }



        public async Task<ClientResponse> GetLeaveTypeEmployee()
        {
            try
            {
                return await _leavetypeEmployeeRepo.GetLeaveTypeEmployee();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
