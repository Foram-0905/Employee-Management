using beONHR.DAL;
using beONHR.Entities;
using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManageLeaveController : ControllerBase
    {
        private readonly IMangeLeaveService _mangeLeave;
        public ManageLeaveController(IMangeLeaveService mangeLeave)
        {
            _mangeLeave = mangeLeave;
        }

        [HttpPost]
        [Route("ApplyLeave")]
        [Authorize("configuration.leave.Add")]
        public async Task<IActionResult> ApplyLeave(ManageLeaveDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _mangeLeave.applyLeave(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [Route("GetFilterLeave")]
        public async Task<ClientResponse> GetFilterLeave(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _mangeLeave.GetFilterLeave(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetLeaveById/{Id}")]
        public async Task<ClientResponse> GetLeaveById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _mangeLeave.GetLeaveById(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        [HttpDelete]
        [Route("DeleteLeave")]
        [Authorize("configuration.manageleave.Delete")]
        public async Task<ClientResponse> DeleteLeave(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _mangeLeave.DeleteLeave(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}