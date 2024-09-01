using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeofEmploymentController : ControllerBase
    {
        private readonly ITypeofEmploymentService _typeofemploymentService;

        public TypeofEmploymentController(ITypeofEmploymentService typeofemploymentService)
        {
            _typeofemploymentService = typeofemploymentService;
        }



        [HttpGet]
        [Route("GetTypeofEmployment")]
        public async Task<IActionResult> GetTypeofEmployment()
        {
            try
            {
                ClientResponse response = await _typeofemploymentService.GetTypeofEmployment();
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
