using beONHR.Entities;
using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesignationController : Controller
    {
        private readonly IDesignationService _designation;
        public DesignationController(IDesignationService designation)
        {
            _designation = designation;
        }

        [HttpPost]
        [Route("SaveDesignation")]
        [Authorize("configuration.designation.Add")]
        public async Task<IActionResult>SaveDesignation(DesignationDto input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _designation.SaveDesignation(input);
                    
                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("GetDesignation")]
        public async Task<ClientResponse> GetDesignation()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _designation.GetDesignation();

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("GetFilterDesignation")]
    
        public async Task<ClientResponse> GetFilterDesignation(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _designation.GetFilterDesignation(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteDesignation/{Id}")]
        [Authorize("configuration.designation.Delete")]
        public async Task<ClientResponse> DeleteDesignation(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _designation.DeleteDesignation(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("getDesignationById/{Id}")]
        public async Task<ClientResponse> GetDesignationById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                
                objresp = await _designation.GetDesignationById(id);

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
