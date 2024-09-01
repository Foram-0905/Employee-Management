using beONHR.Entities;
using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employee;

        public EmployeeController(IEmployeeService employee)
        {
            _employee = employee;
        }

        [HttpPost]
        [Route("SaveEmployee")]
        [Authorize("employeeprofile.personal.Add")]
        public async Task<IActionResult> SaveEmployee(EmployeeDTO request)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _employee.SaveEmployee(request); // , request.LanguageCompetences);
                return Ok(objresp);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving employee");
            }
        }
        [HttpPost]
        [Route("GetFilterEmployee")]
        public async Task<ClientResponse> GetFilterEmployee(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _employee.GetFilterEmployee(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetEmployee")]
        public async Task<IActionResult> GetEmployee()
        {
            try
            {
                ClientResponse response = await _employee.GetEmployee();
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching employees");
            }
        }
        
        [HttpGet]
        [Route("GetEmployeeById/{Id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            try
            {
                ClientResponse response = await _employee.GetEmployeeById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching employees");
            }
        }

        [HttpGet]
        [Route("GetEmployeeByLeader/{Id}")]
        public async Task<IActionResult> GetEmployeeByLeader(string id)
        {
            try
            {
                ClientResponse response = await _employee.GetEmployeeByLeader(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching employees");
            }
        }
        [HttpGet]
        [Route("getEmployeeByHr/{Id}")]
        public async Task<IActionResult> getEmployeeByHr(string id)
        {
            try
            {
                ClientResponse response = await _employee.getEmployeeByHr(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching employees");
            }
        }
        [HttpDelete]
        [Route("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                ClientResponse response = await _employee.DeleteEmployee(id);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting employee");
            }
        }

        [HttpGet]
        [Route("GetAvailableLeaveByEmployeeId/{employeeId}")]
        public async Task<ClientResponse> GetAvailableLeaveByEmployeeId(Guid employeeId)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _employee.GetAvailableLeaveByEmployeeId(employeeId);
                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
