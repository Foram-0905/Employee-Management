using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveTypeController(ILeaveTypeService leaveTypeService)
        {
            _leaveTypeService = leaveTypeService;
        }

        [HttpPost]
        [Route("SaveLeaveType")]
        [Authorize("configuration.leavetype.Add")]

        public async Task<IActionResult> SaveLeaveType(LeaveTypeDTO input)
        {
            try
            {
                var response = await _leaveTypeService.SaveLeaveType(input);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetLeaveType")]
        public async Task<IActionResult> GetLeaveType()
        {
            try
            {
                var response = await _leaveTypeService.GetLeaveType();
                return returnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("GetFilterLeaveType")]
        public async Task<ClientResponse> GetFilterLeaveType(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _leaveTypeService.GetFilterLeaveType(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete]
        [Route("DeleteLeaveType/{id}")]
        [Authorize("configuration.leavetype.Delete")]

        public async Task<IActionResult> DeleteLeaveType(Guid id)
        {
            try
            {
                var response = await _leaveTypeService.DeleteLeaveType(id);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetLeaveTypeById/{id}")]
        public async Task<IActionResult> GetLeaveTypeById(Guid id)
        {
            try
            {
                var response = await _leaveTypeService.GetLeaveTypeById(id);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected IActionResult returnAction(ClientResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
