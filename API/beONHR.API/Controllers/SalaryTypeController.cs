using beONHR.Entities;
using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryTypeController : Controller
    {
        private readonly ISalaryTypeService _salaryTypeService;

        public SalaryTypeController(ISalaryTypeService salaryTypeService)
        {
            _salaryTypeService = salaryTypeService;
        }

        [HttpGet]
        [Route("GetSalaryTypes")]

       
  
        public async Task<ClientResponse> GetSalaryType()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _salaryTypeService.GetSalaryTypes();

                return objresp;
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