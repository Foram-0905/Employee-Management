using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class LeaveTypeDTO
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; }
        public Guid CategoryName { get; set; } //forignkey
        public ActionEnum Action { get; set; }

    }
    public class ResponseLeaveTypeDto
    {
        public List<LeaveType> leaveType { get; set; }
        public int TotalRecord { get; set; }


    }
}
