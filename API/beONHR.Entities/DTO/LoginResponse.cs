using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class LoginResponse
    {
        public Guid Id { get; set; }    
        public string Name { get; set; }
        public string PreferredLanguage { get; set; }
        public string Email { get; set; }
        public string? PreferedLanguage { get; set; }
        public string? Token { get; set; }
        public string? RoleId { get; set; }
        public string? Role{ get; set; }

        public DateTime Expiration { get; set; }
        public DateTime? SignDate { get; set; }

        //public string? PreferedLanguage { get; set; }


    }
}
