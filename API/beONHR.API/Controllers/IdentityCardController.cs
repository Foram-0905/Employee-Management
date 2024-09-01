using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityCardController : Controller
    {
        private readonly IIdentityService _identity;
        public IdentityCardController(IIdentityService identity)
        {
            _identity = identity;
        }

        [HttpPost]
        [Route("SaveIdentityCard")]
        [Authorize("employeeprofile.identitycards.Add")]

        public async Task<IActionResult> SaveIdentity(IdentityCardDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _identity.SaveIdentity(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetIdentityCard")]
        public async Task<ClientResponse> GetIdentity()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _identity.GetIdentity();

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("GetIdentityCardByEmployeeId/{Id}")]
        public async Task<ClientResponse> GetIdentityByEmployeeId(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _identity.GetIdentityByEmployeeId(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetIdentityCardById/{Id}")]
        public async Task<ClientResponse> GetIdentityById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _identity.GetIdentityById(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete]
        [Route("DeleteIdentityCard/{Id}")]
        [Authorize("employeeprofile.identitycards.Delete")]

        public async Task<ClientResponse> DeleteIdentity(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _identity.DeleteIdentity(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
