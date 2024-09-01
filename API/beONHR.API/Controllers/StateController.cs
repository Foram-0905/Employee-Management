using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using beONHR.Entities;
using System.Net;
using static beONHR.Entities.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : Controller
    {

        private readonly IStateService _state;
        public StateController(IStateService state)
        {
            _state = state;
        }

        [HttpPost]
        [Route("Savestate")]
        [Authorize("configuration.stateregion.Add")]
        public async Task<IActionResult> SaveState(StateDto input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _state.SaveState(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        [Route("Getstate")]
        public async Task<ClientResponse> GetState()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _state.GetState();

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("Deletestate/{Id}")]
        [Authorize("configuration.stateregion.Delete")]
        public async Task<ClientResponse> DeleteState(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _state.DeleteState(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetstateById/{Id}")]
        public async Task<ClientResponse> GetStateById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _state.GetStateById(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetStatesByCountryId/{countryId}")]
        public async Task<ClientResponse> GetStatesByCountryId(Guid countryId)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _state.GetStatesByCountryId(countryId);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("GetFilterstate")]
        public async Task<ClientResponse> GetFilterState(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _state.GetFilterState(filterRequset);

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

