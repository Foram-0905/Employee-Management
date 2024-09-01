using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Asset_typeController : ControllerBase
    {
        private readonly IAssets_typeService _asset_typeService;

        public Asset_typeController(IAssets_typeService asset_typeService)
        {
            _asset_typeService = asset_typeService;
        }



        [HttpGet]
        [Route("GetAssetstype")]
        public async Task<IActionResult> GetAssets_type()
        {
            try
            {
                ClientResponse response = await _asset_typeService.GetAssets_type();
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