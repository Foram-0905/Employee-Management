using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Permission;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permission;
        public PermissionController(IPermissionService permission)
        {
            _permission = permission;
        }

        [HttpPost]
        [Route("SetPermission")]
        [Authorize("configuration.permission.Add")]
        public async Task<IActionResult> SetPermission(SetPermissionDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _permission.SetPermission(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetPermissionByRole/{Id}")]
        public async Task<ClientResponse> GetPermissionByRole(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _permission.GetPermissionByRole(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
