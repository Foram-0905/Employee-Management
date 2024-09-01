using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BonusController : ControllerBase
    {
        private readonly IBonusService _bonusService;

        public BonusController(IBonusService bonusService)
        {
            _bonusService = bonusService;
        }

        [HttpPost]
        [Route("SaveBonus")]
        [Authorize("employeeprofile.Bonus.Add")]
        public async Task<IActionResult> SaveBonus(BonusDTO input)
        {
            try
            {
                ClientResponse response = await _bonusService.SaveBonus(input);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                //throw ex;
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet]
        [Route("GetBonus")]
        public async Task<IActionResult> GetBonus()
        {
            try
            {
                ClientResponse response = await _bonusService.GetBonus();
                return returnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetBonusById/{id}")]
        public async Task<IActionResult> GetBonusById(Guid id)
        {
            try
            {
                ClientResponse response = await _bonusService.GetBonusById(id);
                return returnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("GetFilterBonus")]
        public async Task<ClientResponse> GetFilterBonus(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _bonusService.GetFilterBonus(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteBonus/{id}")]
        [Authorize("employeeprofile.Bonus.Delete")]
        public async Task<IActionResult> DeleteBonus(Guid id)
        {
            try
            {
                ClientResponse response = await _bonusService.DeleteBonus(id);
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
