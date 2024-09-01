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

    public interface IMangeLeaveService
    {
        Task<ClientResponse> applyLeave(ManageLeaveDTO input);
        Task<ClientResponse> GetFilterLeave(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetLeaveById(Guid id);
        Task<ClientResponse> DeleteLeave(Guid id);



    }
    public class MangeLeaveService: IMangeLeaveService
    {
        private readonly IMangeLeaveRepo _mangeLeave;
        public MangeLeaveService(IMangeLeaveRepo mangeLeave)
        {
            _mangeLeave = mangeLeave;
        }

        public async Task<ClientResponse> applyLeave(ManageLeaveDTO input)
        {
            try
            {
                return await _mangeLeave.applyLeave(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ClientResponse> GetFilterLeave(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _mangeLeave.GetFilterLeave(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ClientResponse> GetLeaveById(Guid id)
        {
            try
            {
                return await _mangeLeave.GetLeaveById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteLeave(Guid id)
        {
            try
            {
                return await _mangeLeave.DeleteLeave(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
