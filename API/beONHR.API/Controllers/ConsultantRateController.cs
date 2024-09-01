using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantRateController : Controller
    {

        private readonly IConsultantRateService _consultant;
        public ConsultantRateController(IConsultantRateService consultant)
        {
            _consultant = consultant;
        }
        [HttpPost]
        [Route("SaveConsultantRate")]
        public async Task<IActionResult> SaveConsultantRate(ConsultantRateDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _consultant.SaveConsultantRate(input);

                return Ok(objresp);
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
