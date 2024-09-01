using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class EmployeeYearlyLeaveBalanceDTO
    {
        public Guid Id { get; set; }
        public DateOnly LeaveStartDate { get; set; }
        public DateOnly LeaveEndDate { get; set; }
        public Guid LeaveTypeEmployee { get; set; }
        public string LeaveName { get; set; }
        public Guid LeaveTypesEmployee { get; set; }
        public Guid EmployeeId { get; set; }

        public float LeaveQuota { get; set; }
        public bool AdjustedEndDate { get; set; }
       
    }
}
