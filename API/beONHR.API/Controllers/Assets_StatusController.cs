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
    
    public class Asset_StatusController : ControllerBase
    {
        private readonly IAssets_StatusService _asset_statusService;

        public Asset_StatusController(IAssets_StatusService asset_statusService)
        {
            _asset_statusService = asset_statusService;
        }



        [HttpGet]
        [Route("GetAssetsStatus")]
        public async Task<IActionResult> GetAssets_Status()
        {
            try
            {
                ClientResponse response = await _asset_statusService.GetAssets_Status();
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