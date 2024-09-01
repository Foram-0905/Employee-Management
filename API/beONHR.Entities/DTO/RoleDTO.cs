using beONHR.Entities.DTO.Enum;
using beONHR.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class RoleDTO
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Description{ get; set; }
        public ActionEnum Action { get; set; }
    }
    public class ResponseRoleDto
    {
        public List<AspNetRoles> Roles { get; set; }
        public int TotalRecord { get; set; }


    }
}
