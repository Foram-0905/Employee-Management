using beONHR.Entities;
using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageLevelController : ControllerBase
    {
        private readonly ILanguageLevelService _languageLevel;

        public LanguageLevelController(ILanguageLevelService languagelevel)
        {
            _languageLevel = languagelevel;
        }

        [HttpPost]
        [Route("SaveLanguagelevel")]
        [Authorize("configuration.languagelevel.Add")]
        public async Task<IActionResult> SaveLanguageLevel(LanguageLevelDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _languageLevel.SaveLanguageLevel(input);

                return returnAction(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetLanguagelevel")]
        public async Task<IActionResult> GetLanguageLevel()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _languageLevel.GetLanguageLevel();

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteLanguagelevel/{Id}")]
        [Authorize("configuration.languagelevel.Delete")]
        public async Task<ClientResponse> DeleteLanguageLevel(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _languageLevel.DeleteLanguageLevel(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("GetLanguagelevelById/{Id}")]
        public async Task<ClientResponse> GetLanguageLevelById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _languageLevel.GetLanguageLevelById(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("GetFilterLanguagelevel")]
        public async Task<ClientResponse> GetFilterLanguageLevel(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _languageLevel.GetFilterLanguageLevel(filterRequset);

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