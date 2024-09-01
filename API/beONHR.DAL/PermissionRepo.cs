using beONHR.Entities.DTO.ForgotPassword;
using beONHR.Entities.DTO;
using beONHR.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using beONHR.Entities.DTO.Permission;
using System.Security.Claims;
using System.Security;
using static beONHR.Entities.Permissions;
using System.Net;

namespace beONHR.DAL
{
    public interface IPermissionRepo
    {
        Task<ClientResponse> SetPermission(SetPermissionDTO input);
        Task<ClientResponse> GetPermissionByRole(Guid id);
    }
    public class PermissionRepo : IPermissionRepo
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly RoleManager<AspNetRoles> _roleManager;
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _configuration;

        public PermissionRepo(UserManager<AspNetUsers> userManager,
            RoleManager<AspNetRoles> roleManager,
            IConfiguration configuration, IUserRepo userRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ClientResponse> SetPermission(SetPermissionDTO input)
        {
            ClientResponse response = new ClientResponse();
            var res = IdentityResult.Success;
            try
            {
                if (input != null)
                {
                    var role = await _roleManager.FindByIdAsync(input.role.ToString());
                    if (role == null)
                    {
                        response.Message = "Role not Exists";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;

                        return response;
                    }
                    //role = await _roleManager.FindByNameAsync(input.role);
                    var calims = await _roleManager.GetClaimsAsync(role);
                    if (calims.Any())
                    {
                        foreach (var claim in calims)
                        {
                            if (!input.permissions.Contains(claim.Value))
                            {
                                await _roleManager.RemoveClaimAsync(role, claim);
                            }
                        }
                    }
                    foreach (var item in input.permissions)
                    {
                        if (calims.ToList().Find(c => c.Value == item) == null)
                        {
                            res = await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, item));
                        }
                    }
                    if (res != IdentityResult.Success)
                    {
                        response.Message = "Permission not insert";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Permission insert Sucesfully";
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ClientResponse> GetPermissionByRole(Guid id)
        {
            ClientResponse response = new ClientResponse();
            var res = IdentityResult.Success;
            try
            {
                if (id != null)
                {
                    var role = await _roleManager.FindByIdAsync(id.ToString());
                    if (role == null)
                    {
                        response.Message = "Role not Exists";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;

                        return response;
                    }
                    //role = await _roleManager.FindByNameAsync(input.role);
                    var calims = await _roleManager.GetClaimsAsync(role);
                    var claimValues = calims.Select(claim => claim.Value).ToList();
                    if (calims.Any())
                    {
                       response.Message = "Permission Get";
                        response.StatusCode =HttpStatusCode.OK;
                        response.HttpResponse = claimValues;
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Message = "Not Exists Any Permission";
                        response.StatusCode = HttpStatusCode.OK;
                        response.HttpResponse = claimValues;
                        response.IsSuccess = true;
                    }
                }
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
