using beONHR.Entities;
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
    //[Authorize]
    public class JobHistoryController : ControllerBase
    {
        private readonly IJobHistoryService _jobHistoryService;

        public JobHistoryController(IJobHistoryService jobHistoryService)
        {
            _jobHistoryService = jobHistoryService;
        }

        [HttpPost]
        [Route("SaveJoBHistory")]
        [Authorize("employeeprofile.jobhistory.Add")]

        public async Task<IActionResult> SaveJobHistory(JobHistoryDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _jobHistoryService.SaveJobHistory(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("GetJobHistory")]
        public async Task<ClientResponse> GetJobHistory()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _jobHistoryService.GetJobHistory();

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("GetFilterGetJobHistory")]
        public async Task<ClientResponse> GetFilterJobHistory(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _jobHistoryService.GetFilterJobHistory(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetJobHistoryById/{id}")]
        public async Task<ClientResponse> GetJobHistoryById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _jobHistoryService.GetJobHistoryById(id);

                return objresp;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

            [HttpGet]
            [Route("GetJobHistoryByEmployeeId/{id}")]
            public async Task<ClientResponse> GetJobHistoryByEmployeeId(Guid id)
            {
                ClientResponse objresp = new ClientResponse();
                try
                {
                    objresp = await _jobHistoryService.GetJobHistoryByEmployeeId(id);

                    return objresp;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        


        [HttpDelete]
        [Route("DeleteJobHistory/{Id}")]
        [Authorize("employeeprofile.jobhistory.Delete")]
        public async Task<ClientResponse> DeleteJobHistory(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _jobHistoryService.DeleteJobHistory(id);

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