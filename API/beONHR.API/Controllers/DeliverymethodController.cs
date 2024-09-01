using beONHR.DAL;
using beONHR.Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliverymethodController : ControllerBase
    {

        private readonly IDeliverymethodRepo _deliverymethodRepo;
        public DeliverymethodController(IDeliverymethodRepo deliverymethodRepo)
        {
            _deliverymethodRepo = deliverymethodRepo;
        }
        [HttpGet]
        [Route("GetDeliverymethod")]
        public async Task<IActionResult> GetDeliverymethod()
        {
            try
            {
                ClientResponse response = await _deliverymethodRepo.GetDeliverymethod();
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
