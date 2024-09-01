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
    //[Authorize]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService _educationService;

        public EducationController(IEducationService educationService)
        {
            _educationService = educationService;
        }

        [HttpPost]
        [Route("SaveEducation")]
        [Authorize("employeeprofile.education.Add")]
        public async Task<IActionResult> SaveEducation(EducationDTO input)
        {
            try
            {
                ClientResponse response = await _educationService.SaveEducation(input);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                //throw ex;
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet]
        [Route("GetEducation")]
        public async Task<IActionResult> GetEducation()
        {
            try
            {
                ClientResponse response = await _educationService.GetEducation();
                return returnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetEducationById/{id}")]
        public async Task<IActionResult> GetEducationById(Guid id)
        {
            try
            {
                ClientResponse response = await _educationService.GetEducationById(id);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetEducationByEmployee/{employeeId}")]
        public async Task<IActionResult> GetEducationByEmployee(Guid employeeId)
        {
            try
            {
                ClientResponse response = await _educationService.GetEducationByEmployee(employeeId);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
        [HttpPost]
        [Route("GetFilterEducation")]
        public async Task<ClientResponse> GetFilterEducation(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _educationService.GetFilterEducation(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteEducation/{id}")]
        [Authorize("employeeprofile.education.Delete")]
        public async Task<IActionResult> DeleteEducation(Guid id)
        {
            try
            {
                ClientResponse response = await _educationService.DeleteEducation(id);
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