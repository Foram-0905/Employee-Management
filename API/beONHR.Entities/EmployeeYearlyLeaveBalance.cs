using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class EmployeeYearlyLeaveBalance
    {
        [Key]
        public Guid Id { get; set; }
        public DateOnly LeaveStartDate {  get; set; }
        public DateOnly LeaveEndDate { get; set; }
        public Guid LeaveTypeEmployee { get; set; }

        [ForeignKey("LeaveTypeEmployee")]
        public virtual LeaveType? LeaveType { get; set; }

        public Guid LeaveTypesEmployee { get; set; }

        [ForeignKey("LeaveTypesEmployee")]
        public virtual LeaveTypeEmployee? LeaveTypesEmployees { get; set; }
        public float LeaveQuota { get; set; }
        public Guid EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
        public bool AdjustedEndDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
