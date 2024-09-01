
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class IdentityCard
    {
        [Key]
        public Guid Id { get; set; }
        public string Passport { get; set; }
        public DateOnly VisaStartdate { get; set; }
        public DateOnly VisaExpirytdate { get; set; }
        public string Visa { get; set; }
        public DateOnly BlueCardStartdate { get; set; }
        public DateOnly BlueCardExpirytdate { get; set; }
        public string BlueCard { get; set; }

        public Guid EmployeeId { get; set; } // foreign key
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
        public virtual ICollection<WorkPermitDetail> WorkPermitDetails { get; set; }
    }
}

namespace beONHR.Entities
{
    public class WorkPermitDetail
    {
        [Key]
        public Guid Id { get; set; }
        public string PermitType { get; set; }
        public DateOnly PermitStartdate { get; set; }
        public DateOnly PermitExpirytdate { get; set; }
        public Guid IdentityId { get; set; }//foriegnkey
        public string Document { get; set; }

        [ForeignKey("IdentityId")]
        public virtual IdentityCard? identity { get; set; }

    }
}


