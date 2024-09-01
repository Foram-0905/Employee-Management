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
    public class SalaryController : Controller
    {
        private readonly ISalaryService _salary;
        public SalaryController(ISalaryService salary)
        {
            _salary = salary;
        }


        [HttpPost]
        [Route("SaveSalary")]
        [Authorize("employeeprofile.Salary.Add")]

        public async Task<IActionResult> SaveSalary(SalaryDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _salary.SaveSalary(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetSalary")]
        public async Task<ClientResponse> GetSalary()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _salary.GetSalary();

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete]
        [Route("DeleteSalary/{Id}")]
        [Authorize("employeeprofile.Salary.Delete")]

        public async Task<ClientResponse> DeleteSalary(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _salary.DeleteSalary(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetPreviousMonthSalary")]
        public async Task<ClientResponse> GetPreviousMonthSalary(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _salary.GetPreviousMonthSalary(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetTwoMonthsAgoSalary")]
        public async Task<ClientResponse> GetTwoMonthsAgoSalary(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _salary.GetTwoMonthsAgoSalary(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("GetFilterSalary")]
        public async Task<ClientResponse> GetFilterSalary(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _salary.GetFilterSalary(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetSalaryById/{Id}")]
        public async Task<ClientResponse> GetSalaryById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _salary.GetSalaryById(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetSalaryByEmployee")]
        public async Task<ClientResponse> GetSalaryByEmployee(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _salary.GetSalaryByEmployee(id);

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
