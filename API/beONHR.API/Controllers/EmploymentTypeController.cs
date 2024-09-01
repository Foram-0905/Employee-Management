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
    public class EmploymentTypeController : ControllerBase
    {
        private readonly IEmploymentTypeRepo _employmenttypeRepo;
        public EmploymentTypeController(IEmploymentTypeRepo maritalrepo)
        {
            _employmenttypeRepo = maritalrepo;
        }

        [HttpGet]
        [Route("GetEmploymentType")]
        public async Task<IActionResult> GetEmploymentType()
        {
            try
            {
                ClientResponse response = await _employmenttypeRepo.GetEmploymentType();
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
