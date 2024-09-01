using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface ILeaveTypeService
    {
        Task<ClientResponse> SaveLeaveType(LeaveTypeDTO input);
        Task<ClientResponse> GetLeaveType();
        Task<ClientResponse> GetFilterLeaveType(FilterRequsetDTO filterRequset);

        Task<ClientResponse> DeleteLeaveType(Guid id);
        Task<ClientResponse> GetLeaveTypeById(Guid id);
    }

    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly ILeaveTypeRepo _leaveTypeRepo;

        public LeaveTypeService(ILeaveTypeRepo leaveTypeRepo)
        {
            _leaveTypeRepo = leaveTypeRepo;
        }

        public async Task<ClientResponse> SaveLeaveType(LeaveTypeDTO input)
        {
            try
            {
                return await _leaveTypeRepo.SaveLeaveType(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetLeaveType()
        {
            try
            {
                return await _leaveTypeRepo.GetLeaveType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterLeaveType(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _leaveTypeRepo.GetFilterLeaveType(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteLeaveType(Guid id)
        {
            try
            {
                return await _leaveTypeRepo.DeleteLeaveType(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetLeaveTypeById(Guid id)
        {
            try
            {
                return await _leaveTypeRepo.GetLeaveTypeById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
