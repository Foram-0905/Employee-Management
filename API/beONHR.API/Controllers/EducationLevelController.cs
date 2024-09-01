using beONHR.Entities;
using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EducationLevelController : ControllerBase
    {
        private readonly IEductionLevelService _educationLevel;

        public EducationLevelController(IEductionLevelService educationLevel)
        {
            _educationLevel = educationLevel;
        }

        [HttpPost]
        [Route("SaveEducationLevel")]
        [Authorize("configuration.educationlevel.Add")]
        public async Task<IActionResult> SaveEductionLevel(EductionLevelDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _educationLevel.SaveEductionLevel(input);

                return ReturnAction(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetEducationLevel")]
        public async Task<IActionResult> GetEductionLevel()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _educationLevel.GetEductionLevel();

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteEducationLevel/{Id}")]
        [Authorize("configuration.educationlevel.Delete")]
        public async Task<ClientResponse> DeleteEductionLevel(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _educationLevel.DeleteEductionLevel(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetEducationLevelById/{Id}")]
        public async Task<ClientResponse> GetEductionLevelById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _educationLevel.GetEductionLevelById(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("GetFilterEducationLevel")]
        public async Task<ClientResponse> GetFilterEductionLevel(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _educationLevel.GetFilterEductionLevel(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected IActionResult ReturnAction(ClientResponse objresp)
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