using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class WorkPermit
    {
        [Key]
        public Guid Id { get; set; }
        public string PermitType { get; set; }
        public DateOnly PermitStartdate { get; set; }
        public DateOnly PermitExpirytdate { get; set; }
        public string Document { get; set; }

        public bool IsDeleted { get; set; } = false;
        public Guid EmployeeId { get; set; } //forignkey

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
    }
}
