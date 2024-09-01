using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
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
    public class LeaveCategoryController : ControllerBase
    {
        private readonly ILeaveCategoryService _leaveCategoryService;

        public LeaveCategoryController(ILeaveCategoryService leavecategoryService)
        {
            _leaveCategoryService = leavecategoryService;
        }

        [HttpPost]
        [Route("SaveLeaveCategory")]
        [Authorize("configuration.leavecategory.Add")]
        public async Task<IActionResult> SaveLeavecategory(LeaveCategoryDTO input)
        {
            try
            {
                var response = await _leaveCategoryService.SaveLeaveCategory(input);
                return ReturnAction(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while saving  LeaveCategory.", ex);
            }
        }

        [HttpGet]
        [Route("GetLeaveCategory")]
        public async Task<IActionResult> GetLeaveCategory()
        {

            try
            {
                var response = await _leaveCategoryService.GetLeaveCategory();
                return ReturnAction(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving  leavecategory.", ex);
            }
        }
   
        [HttpGet]
        [Route("GetLeaveCategoryById/{id}")]
        public async Task<ClientResponse> GetLeaveCategoryById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _leaveCategoryService.GetLeaveCategoryById(id);

                return objresp;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("GetFilterLeaveCategory")]
        public async Task<ClientResponse> GetFilterLeaveCategory(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _leaveCategoryService.GetFilterLeaveCategory(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteLeaveCategory/{id}")]
        [Authorize("configuration.leavecategory.Delete")]
        public async Task<IActionResult> DeleteLeaveCategory(Guid id)
        {
            try
            {
                var response = await _leaveCategoryService.DeleteLeaveCategory(id);
                return ReturnAction(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected IActionResult ReturnAction(ClientResponse objresp)
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



