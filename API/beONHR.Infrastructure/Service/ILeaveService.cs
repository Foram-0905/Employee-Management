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
    public interface ILeaveService
    {
        Task<ClientResponse> GetLeaveByEmployee(leavesAccordingLogin input);
        Task<ClientResponse> applyLeave(ManageLeaveDTO input);
        Task<ClientResponse> GetAllLeaveByEmployee();
        Task<ClientResponse> ApprovOrRejectLeave(ApprovOrRejectLeave input);
        Task<ClientResponse> GetFilterLeaveHistory(leavesAccordingLogin filterRequset);
        Task<ClientResponse> GetFilterPendingLeave(leavesAccordingLogin filterRequset);
        Task<ClientResponse> GetLeaveByDate(leaveAccordingDate filterRequset);
    }
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepo _leave;
        public LeaveService(ILeaveRepo leave)
        {
            _leave = leave;
        }

        public async Task<ClientResponse> applyLeave(ManageLeaveDTO input)
        {
            try
            {
                return await _leave.applyLeave(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetLeaveByEmployee(leavesAccordingLogin input)
        {
            try
            {
                return await _leave.GetLeaveByEmployee(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterLeaveHistory(leavesAccordingLogin filterRequset)
        {
            try
            {
                return await _leave.GetFilterLeaveHistory(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterPendingLeave(leavesAccordingLogin filterRequset)
        {
            try
            {
                return await _leave.GetFilterPendingLeave(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetAllLeaveByEmployee()
        {
            try
            {
                return await _leave.GetAllLeaveByEmployee();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> ApprovOrRejectLeave(ApprovOrRejectLeave input)
        {
            try
            {
                return await _leave.ApprovOrRejectLeave(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ClientResponse> GetLeaveByDate(leaveAccordingDate filterRequset)
        {
            try
            {
                return await _leave.GetLeaveByDate(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
