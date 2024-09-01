using beONHR.DAL;
using beONHR.Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeEmployeeController : ControllerBase
    {
        private readonly ILeaveTypeEmployeeRepo _leavetypeEmployeeRepo;

        public LeaveTypeEmployeeController(ILeaveTypeEmployeeRepo leavetypeEmployeeRepo)
        {
            _leavetypeEmployeeRepo = leavetypeEmployeeRepo;
        }

        [HttpGet]
        [Route("GetLeaveTypeEmployee")]
        public async Task<IActionResult> GetLeaveTypeEmployee()
        {
            try
            {
                ClientResponse response = await _leavetypeEmployeeRepo.GetLeaveTypeEmployee();
                return returnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected IActionResult returnAction(ClientResponse objresp)
        {
            if (objresp.StatusCode == HttpStatusCode.OK || objresp.StatusCode == HttpStatusCode.NoContent)
            {
                return Ok(objresp);
            }
            else
            {
                return BadRequest(objresp);
            }
        }
    }
}
