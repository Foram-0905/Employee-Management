using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class Bonus
    {
        [Key]
        public Guid Id { get; set; }
        public bool Entitlement { get; set; }
        public DateOnly StartingDate { get; set; }
        public DateOnly EndingDate { get; set; }
        public string Bonusamount { get; set; }
        public Guid SalaryType { get; set; }
        [ForeignKey("SalaryType")]
        public virtual SalaryType? Salarytype { get; set; }
        public Guid EmployeeId { get; set; } //forignkey
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
    }
}
