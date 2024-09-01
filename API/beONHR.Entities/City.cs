using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    
    public class City
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public Guid CountryId { get; set; } //froginkey
        public Guid State { get; set; } //froginkey
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("State")]
        public virtual State? stateId { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country? country { get; set; }
    }
}
