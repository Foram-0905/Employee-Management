using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class OrganisationalchartDTO
    {
        public string? EmployeeName { get; set; }
        public string? EmployeementType { get; set; }
        public string? SlgName{ get; set; }
        public string? Designation{ get; set; }
    }
    public class GroupedRecordViewModel
    {
        public string Group { get; set; }
        public List<OrganisationalchartDTO> Names { get; set; }
    }
}
