//using beONHR.Entities.DTO;
//using beONHR.Infrastructure.Service;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace beONHR.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LanguageCompetenceController : ControllerBase
//    {
//        private readonly ILanguageCompetenceService _languageCompetenceService;

//        public LanguageCompetenceController(ILanguageCompetenceService languageCompetenceService)
//        {
//            _languageCompetenceService = languageCompetenceService;
//        }

//        [HttpPost]
//        [Route("SaveLanguageCompetence")]
//        public async Task<IActionResult> SaveLanguageCompetence(List<LanguageCompetenceDTO> inputs)
//        {
//            try
//            {
//                var responses = new List<ClientResponse>();
//                foreach (var input in inputs)
//                {
//                    var response = await _languageCompetenceService.SaveLanguageCompetence(input);
//                    responses.Add(response);
//                }
//                return Ok(responses);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        [HttpGet]
//        [Route("GetLanguageCompetence")]
//        public async Task<IActionResult> GetLanguageCompetence()
//        {
//            try
//            {
//                var response = await _languageCompetenceService.GetLanguageCompetence();
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        [HttpDelete]
//        [Route("DeleteLanguageCompetence/{Id}")]
//        public async Task<IActionResult> DeleteLanguageCompetence(Guid id)
//        {
//            try
//            {
//                var response = await _languageCompetenceService.DeleteLanguageCompetence(id);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        [HttpGet]
//        [Route("GetLanguageCompetenceByEmployeeId/{employeeId}")]
//        public async Task<IActionResult> GetLanguageCompetenceByEmployeeId(Guid employeeId)
//        {
//            try
//            {
//                var response = await _languageCompetenceService.LanguageCompetenceGetByEmployeeIdAsync(employeeId);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }
//    }
//}
