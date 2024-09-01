using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicHolidayController : Controller
    {
        private readonly IPublicHolidayService _publicHoliday;

        public PublicHolidayController(IPublicHolidayService publicHoliday)
        {
            _publicHoliday = publicHoliday;
        }

        [HttpPost]
        [Route("SavePublicHoliday")]
        [Authorize("configuration.managepublicholidays.Add")]

        public async Task<IActionResult> SavePublicHoliday(PublicHolidayDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _publicHoliday.SavePublicHoliday(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetPublicHoliday")]
        public async Task<ClientResponse> GetPublicHoliday()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _publicHoliday.GetPublicHoliday();

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("GetFilterPublicHoliday")]
        public async Task<ClientResponse> GetFilterPublicHoliday(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _publicHoliday.GetFilterPublicHoliday(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeletePublicHoliday/{id}")]
        [Authorize("configuration.managepublicholidays.Delete")]

        public async Task<ClientResponse> DeletePublicHoliday(Guid id)
        {


            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _publicHoliday.DeletePublicHoliday(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetPublicHolidayById/{id}")]
        public async Task<ClientResponse> GetPublicHolidayById(Guid id)
        {


            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _publicHoliday.GetPublicHolidayById(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected IActionResult returnAction(ClientResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}