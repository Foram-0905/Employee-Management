using beONHR.DAL;
using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaritalStatusController : ControllerBase
    {
        private readonly IMaritalStatusService _maritalrepo;
        public MaritalStatusController(IMaritalStatusService maritalrepo)
        {
            _maritalrepo = maritalrepo;
        }

        [HttpGet]
        [Route("GetMaritalStatus")]
        public async Task<IActionResult> GetMaritalStatus()
        {
            try
            {
                ClientResponse response = await _maritalrepo.GetMaritalStatus();
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