using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveController : Controller
    {
        private readonly ILeaveService _leave;

        public LeaveController(ILeaveService leave)
        {
            _leave = leave;
        }


        [HttpPost]
        [Route("ApplyLeave")]
        [Authorize("Leave.leavedetails.Add")]
        public async Task<IActionResult> ApplyLeave(ManageLeaveDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _leave.applyLeave(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("GetLeaveByEmployee")]
        public async Task<ClientResponse> GetLeaveByEmployee(leavesAccordingLogin input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                    objresp = await _leave.GetLeaveByEmployee(input);
                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("GetFilterLeaveHistory")]
        public async Task<ClientResponse> GetFilterLeaveHistory(leavesAccordingLogin input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _leave.GetFilterLeaveHistory(input);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("GetFilterPendingLeave")]
        public async Task<ClientResponse> GetFilterPendingLeave(leavesAccordingLogin input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _leave.GetFilterPendingLeave(input);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetAllLeaveByEmployee")]
        public async Task<ClientResponse> GetAllLeaveByEmployee()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _leave.GetAllLeaveByEmployee();

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("ApprovOrRejectLeave")]
        public async Task<IActionResult> ApprovOrRejectLeave(ApprovOrRejectLeave input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _leave.ApprovOrRejectLeave(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("GetLeaveByDate")]
        public async Task<IActionResult> GetLeaveByDate(leaveAccordingDate filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _leave.GetLeaveByDate(filterRequset);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
