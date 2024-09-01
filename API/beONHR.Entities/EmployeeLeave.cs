using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class EmployeeLeave
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }//froginkey
        public Guid Leavetype { get; set; } //froginkey
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public float LeaveDay { get; set; }
        public DateOnly AppliedDate { get; set; }
        public bool ApprovedbyOfficeManagement { get; set; } = false;
        public string? OfficeManagementName {  get; set; }
        public bool ApprovedbyTeamlead {  get; set; }=false;
        public string? TeamleadName { get; set; }
        public bool IsRejected { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public string? RejectedReason { get; set; }  
        public string? Reason { get; set; }
        public string? leave_duration { get; set; } = "Full";
        public string? Leave_Start_From { get; set; } = "Full";
        public string? Leave_End { get; set; } = "Full";
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [ForeignKey("Leavetype")]
        public virtual LeaveType? LeavetypeId { get; set; }
     

    }
}
