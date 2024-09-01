using beONHR.DAL;
using beONHR.Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeenStatusController : ControllerBase
    {
        private readonly IEmployeenStatusRepo _employmenttypeRepo;
        public EmployeenStatusController(IEmployeenStatusRepo maritalrepo)
        {
            _employmenttypeRepo = maritalrepo;
        }

        [HttpGet]
        [Route("GetEmployeenStatus")]
        public async Task<IActionResult> GetEmployeenStatus()
        {
            try
            {
                ClientResponse response = await _employmenttypeRepo.GetEmployeenStatus();
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
