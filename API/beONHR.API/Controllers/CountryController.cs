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
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _country;

        public CountryController(ICountryService country)
        {
            _country = country;
        }

        [HttpPost]
        [Route("SaveCountry")]
        [Authorize("configuration.managecountry.Add")]

        public async Task<IActionResult> SaveCountry(CountryDTO input)
        {
            try
            {
                var response = await _country.SaveCountry(input);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCountry")]
        public async Task<IActionResult> GetCountry()
        {
            try
            {
                var response = await _country.GetCountry();
                return returnAction(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetCountryById/{Id}")]

        public async Task<ClientResponse> GetCountryById(Guid Id)
        {

            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _country.GetCountryById(Id);
                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("GetFilterCountry")]
        public async Task<ClientResponse> GetFilterCountry(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _country.GetFilterCountry(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete]
        [Route("DeleteCountry/{countryId}")]
        [Authorize("configuration.managecountry.Delete")]

        public async Task<IActionResult> DeleteCountry(Guid countryId)
        {
            try
            {
                var response = await _country.DeleteCountry(countryId);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        protected IActionResult returnAction(ClientResponse objresp)
        {
            if (objresp.IsSuccess)
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
