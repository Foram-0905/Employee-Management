using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class taxclassController : ControllerBase
    {
        private readonly ItaxclassService _taxclassService;

        public taxclassController(ItaxclassService taxclassService)
        {
            _taxclassService = taxclassService;
        }



        [HttpGet]
        [Route("Gettaxclass")]
        public async Task<IActionResult> Gettaxclass()
        {
            try
            {
                ClientResponse response = await _taxclassService.Gettaxclass();
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