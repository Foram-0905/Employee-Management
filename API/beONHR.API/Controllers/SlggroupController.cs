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
    public class SlggroupController : ControllerBase
    {
        private readonly ISlggroupService _slg;

        public SlggroupController(ISlggroupService slg)
        {
            _slg = slg;
        }

        [HttpPost]
        [Route("SaveSlggroup")]
        [Authorize("configuration.slggroup.Add")]
        public async Task<IActionResult> SaveSlggroup(SlggroupDto input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _slg.SaveSlggroup(input);

                return returnAction(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("Getslggroup")]
        public async Task<IActionResult> GetSlggroup()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _slg.GetSlggroup();

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteSlggroup/{Id}")]
        [Authorize("configuration.slggroup.Delete")]
        public async Task<ClientResponse> DeleteSlggroup(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _slg.DeleteSlggroup(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("GetSLGgroupById/{Id}")]
        public async Task<ClientResponse> GetSLGgroupById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _slg.GetSLGgroupById(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("GetFilterSLGgroup")]
        public async Task<ClientResponse> GetFilterSlggroup(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _slg.GetFilterSlggroup(filterRequset);

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

