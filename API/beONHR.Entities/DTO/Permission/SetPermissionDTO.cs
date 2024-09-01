using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO.Permission
{
    public class SetPermissionDTO
    {

        public Guid role { get; set; }
        public List<string> permissions { get; set; }

    }
}
