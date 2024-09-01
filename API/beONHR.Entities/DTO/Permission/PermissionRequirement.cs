using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

    public static class Permissions
    {
        public static class Designation
        {
  
            public const string Create = "Permissions.Designation.Add";
            public const string Update = "Permissions.Designation.Update";
        
        }


        public class CustomClaimTypes
        {
            public const string Permission = "permission";
        }
    }
}
