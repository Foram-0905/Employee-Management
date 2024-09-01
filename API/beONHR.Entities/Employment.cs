using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class Employment
    {
        [Key]
        public Guid Id { get; set; }
        public DateOnly NoticePeriodStartDate { get; set; }
        public DateOnly NoticePeriodEndDate { get; set; }
        public DateOnly TerminationEndDate { get; set; }
        public DateOnly TerminationStartDate { get; set; }
        public string TerminationEmplyee {  get; set; }
        public Guid EmployeeId { get; set; } //forigenkey
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
    }
}
