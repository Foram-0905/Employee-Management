using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    internal class WorkPermitDTO
    {
        public Guid Id { get; set; }
        public string PermitType { get; set; }
        public DateOnly PermitStartdate { get; set; }
        public DateOnly PermitExpirytdate { get; set; }
        public string Document { get; set; }
        public Guid EmployeeId { get; set; } //forignkey
    }
}
