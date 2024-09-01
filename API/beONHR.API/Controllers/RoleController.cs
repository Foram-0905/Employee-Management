using beONHR.Entities.DTO;
using beONHR.Entities.User;
using beONHR.Infrastructure.Service;
using beONHR.Infrastructure.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : Controller
    {

        private readonly UserManager<AspNetUsers> _userManager;
        //private readonly RoleManager<AspNetRoles> _roleManager;
        private readonly IRoleService _role;
        public RoleController(UserManager<AspNetUsers> userManager, IRoleService role, RoleManager<AspNetRoles> roleManager)
        {
            _userManager = userManager;
            //_roleManager = roleManager;
            _role = role;
        }
        [HttpPost]
        [Route("SaveRole")]
        [Authorize("configuration.role.Add")]
        public async Task<IActionResult> SaveRole(RoleDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _role.SaveRole(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetRole")]
        public async Task<ClientResponse> GetRole()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _role.GetRole();

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetRoleByid/{id}")]
        public async Task<ClientResponse> GetRoleByid(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _role.GetRoleById(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("GetFilterRole")]
        public async Task<ClientResponse> GetFilterDesignation(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _role.GetFilterRole(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteRole/{id}")]
        [Authorize("configuration.role.Delete")]
        public async Task<ClientResponse> DeleteRole(string id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _role.DeleteRole(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}