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
    
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("SaveNotification")]
        //[Authorize("configuration.notification.Add")]
        public async Task<IActionResult> SaveNotification(NotificationDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _notificationService.SaveNotification(input);
                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("UpdateNotification")]
        public async Task<IActionResult> UpdateNotification(NotificationDTO input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _notificationService.UpdateNotification(input);

                if (!response.IsSuccess)
                {
                    return StatusCode((int)response.StatusCode, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }





        [HttpGet]
        [Route("GetNotification")]
        public async Task<ClientResponse> GetNotification()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _notificationService.GetNotification();
                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetNotificationByEmployeeId/{employeeId}")]
        public async Task<ClientResponse> GetNotificationByEmployeeId(Guid employeeId)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _notificationService.GetNotificationByEmployeeId(employeeId);
                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("GetNotificationId/{notificationID}")]
        public async Task<ClientResponse> GetNotificationId(Guid notificationID)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _notificationService.GetNotificationId(notificationID);
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
