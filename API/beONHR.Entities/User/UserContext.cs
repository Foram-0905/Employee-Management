using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.User
{

    public class AspNetUsers : IdentityUser
    {
        public bool IsDeleted { get; set; } = false;
        public string PreferredLanguage { get; set; }="en";
    }

    public class AspNetRoles : IdentityRole
    {
        public string Description { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
    public class UserContext : IdentityDbContext<AspNetUsers, AspNetRoles, string>
    {
        public DbSet<AspNetUsers> ApplicationUsers { get; set; }
        public DbSet<AspNetRoles> AspNetRoles { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }


    }

}
