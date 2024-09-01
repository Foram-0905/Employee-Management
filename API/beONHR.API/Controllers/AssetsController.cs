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
    [Authorize]
    public class AssetController : ControllerBase
    {
        private readonly IAssetsService _assetService;

        public AssetController(IAssetsService assetService)
        {
            _assetService = assetService;
        }

        [HttpPost]
        [Route("SaveAsset")]
        [Authorize("configuration.manageasset.Add")]
        public async Task<IActionResult> SaveAsset(AssetsDTO input)
        {
            try
            {
                ClientResponse response = await _assetService.SaveAsset(input);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                //throw ex;
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet]
        [Route("GetAssets")]
        public async Task<IActionResult> GetAssets()
        {
            try
            {
                ClientResponse response = await _assetService.GetAssets();
                return returnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetAssetById/{id}")]
        public async Task<IActionResult> GetAssetById(Guid id)
        {
            try
            {
                ClientResponse response = await _assetService.GetAssetById(id);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("GetFilterAsset")]

        public async Task<ClientResponse> GetFilterAssets(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _assetService.GetFilterAssets(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteAsset/{id}")]
        [Authorize("configuration.manageasset.Delete")]
        public async Task<IActionResult> DeleteAsset(Guid id)
        {
            try
            {
                ClientResponse response = await _assetService.DeleteAsset(id);
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
