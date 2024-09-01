using Azure.Core;
using beONHR.Entities;
using beONHR.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace beONHR.Infrastructure
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        UserManager<AspNetUsers> _userManager;
        RoleManager<AspNetRoles> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public PermissionAuthorizationHandler(UserManager<AspNetUsers> userManager, RoleManager<AspNetRoles> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize]
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            try
            {

                if ((context.User == null) || (context.User.Identity == null) || (context.User != null && context.User.Identity != null && !context.User.Identity.IsAuthenticated))
                {
                    return;
                }


                var userid = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

                var user = await _userManager.FindByEmailAsync(userid);

                var userRoleNames = await _userManager.GetRolesAsync(user);

                var userRoles = await _roleManager.FindByNameAsync(userRoleNames[0]);

                var roleClaims = await _roleManager.GetClaimsAsync(userRoles);

                var permissions = roleClaims.Where(x => x.Type == "permission" &&
                                                        x.Value == requirement.Permission &&
                                                        x.Issuer == "LOCAL AUTHORITY")
                                            .Select(x => x.Value);

                if (permissions.Any())
                {
                    context.Succeed(requirement);
                    return;
                }
                else
                {
                    context.Fail();
                    return;
                }



                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
