using beONHR.Entities.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class Register
    {
        public string? username { get; set; }
        public string? password { get; set; }
        public string? Email { get; set; }
        public string? Role{ get; set;}
    }

    public class forgotmail
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
}
