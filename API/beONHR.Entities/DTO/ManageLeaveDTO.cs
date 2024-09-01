using beONHR.Entities.DTO.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class ManageLeaveDTO
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }//froginkey

        [JsonProperty("leaveType")]
        public Guid Leavetype { get; set; } //froginkey

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public float LeaveDay { get; set; }
        public DateOnly? AppliedDate { get; set; }
        public bool? ApprovedbyOfficeManagement { get; set; } = false;
        public string? OfficeManagementName { get; set; }
        public bool? ApprovedbyTeamlead { get; set; } = false;
        public string? TeamleadName { get; set; }
        public bool? IsRejected { get; set; } = true;
        public string? RejecteReson { get; set; }
        public string? Leave_Start_From { get; set; } 
        public string? Leave_End { get; set; }
        public string? Reason { get; set; }
        public string? leave_duration { get; set; }
        public ActionEnum Action { get; set; }
    }


    public class responseManageLeave
    {
        public List<ManageLeaveList> leaves1 { get; set; }
        public List<EmployeeLeave> leaves { get; set; }
        public int TotalRecord { get; set; }
    }

    public class ManageLeaveList
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }//froginkey
        public string EmployeeName { get; set; }
        public Guid? Leader1 { get; set; }
        public Guid? Leader2 { get; set; }
        public Guid? Leader3 { get; set; }
        [JsonProperty("leaveType")]
        public Guid Leavetype { get; set; } //froginkey
        public string? LeavetypeName {  get; set; }
        public float LeaveDay { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly AppliedDate { get; set; }
        public bool ApprovedbyOfficeManagement { get; set; }
        public string? OfficeManagementName { get; set; }
        public bool ApprovedbyTeamlead { get; set; } 
        public string? TeamleadName { get; set; }
        public bool IsRejected { get; set; }
        public bool IsApproved { get; set; }
        public string? RejectedReason { get; set; }
        public string? Reason { get; set; }
        public string? leave_duration { get; set; }
        public string? Leave_Start_From { get; set; } 
        public string? Leave_End { get; set; }
    }
    public class responseLeave
    {
        public List<EmployeeLeave>? pendingLeaves { get; set; }
        public List<EmployeeLeave>? LeavesHistory { get; set; }
        public List<EmployeeLeave>? allLeave { get; set; }
        
    }

    public class ApprovOrRejectLeave
    {
        public Guid Id { get; set; }
        public string approvedBy { get; set; }
        public string approvedByName { get; set; }
        public string approvedOrreject { get; set; }
        public string? RejecteReson { get; set; }

    }


    public class leavesAccordingLogin
    {
        public string pageType { get; set; }
        public string? Id { get; set; }
        public FilterRequsetDTO filterRequset { get; set; }
    }
    public class leaveAccordingDate
    {
        public string Date { get; set; }
        public string? Id { get; set; }
        public FilterRequsetDTO filterRequset { get; set; }
    }
}
