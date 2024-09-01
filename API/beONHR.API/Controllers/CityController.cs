using beONHR.Entities;
using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Net;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService  )
        {
            _cityService = cityService;
        }

        [HttpPost]
        [Route("SaveCity")]
        [Authorize("configuration.managecity.Add")]
        public async Task<IActionResult> SaveCity(CityDto input)
        {
            try
            {
                var response = await _cityService.SaveCity(input);
                return ReturnAction(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while saving city.", ex);
            }
        }

        [HttpGet]
        [Route("GetCity")]
        public async Task<IActionResult> GetCity()
        {

            try
            {
                var response = await _cityService.GetCity();
                return ReturnAction(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("GetFilterCity")]
        public async Task<ClientResponse> GetFilterCity(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _cityService.GetFilterCity(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetCityById/{id}")]
        public async Task<ClientResponse> GetCityById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _cityService.GetCityById(id);

                return objresp;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetCityByState/{stateId}")]
        public async Task<IActionResult> GetCityByState(Guid stateId)
        {
            try
            {
                var response = await _cityService.GetCityByState(stateId);
                return ReturnAction(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpDelete]
        [Route("DeleteCity/{id}")]
        [Authorize("configuration.managecity.Delete")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            try
            {
                var response = await _cityService.DeleteCity(id);
                return ReturnAction(response);
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
