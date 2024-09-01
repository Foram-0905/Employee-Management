using beONHR.Entities;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganisationalchartController : ControllerBase
    {
        private readonly IOrganisationalchartService _organisationalchartService;
        public OrganisationalchartController(IOrganisationalchartService organisationalchartService)
        {
            _organisationalchartService = organisationalchartService;
        }

        [HttpGet]
        [Route("GetEmployeeForChart")]
        public async Task<IActionResult> GetEmployeeForChart()
        {
            try
            {
                var response = await _organisationalchartService.GetEmployeeForChart();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
