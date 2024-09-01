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
    [Authorize]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpPost]
        [Route("SaveCurrency")]
        [Authorize("configuration.managecurrency.Add")]
        public async Task<IActionResult> SaveCurrency(CurrencyDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _currencyService.SaveCurrency(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetCurrency")]
        public async Task<ClientResponse> GetCurrency()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _currencyService.GetCurrency();

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("getCurrencyById/{id}")] // Corrected route template to match parameter name
        public async Task<ClientResponse> getCurrencyById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _currencyService.GetCurrencyById(id); // Corrected method call

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("GetFilterCurrency")]
        public async Task<ClientResponse> GetFilterCurrency(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _currencyService.GetFilterCurrency(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpDelete]
        [Route("DeleteCurrency/{id}")]
        [Authorize("configuration.managecurrency.Delete")]
        public async Task<ClientResponse> DeleteCurrency(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _currencyService.DeleteCurrency(id);

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
