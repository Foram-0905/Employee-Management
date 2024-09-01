using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class JobHistoryDTO
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string PositionHeld { get; set; }
        public Guid EmploymentType { get; set; }
        public string EmploymentTypeName { get; set; }
        public int ZipCode { get; set; }
        public Guid City { get; set; } 
        public string CityName { get; set; }
        public Guid Country { get; set; }
        public string CountryName { get; set; }
        public Guid StateId { get; set; }
        public string StateName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Document { get; set; }
        public string LeavingReason { get; set; }
        public Guid EmployeeId { get; set; } 
        public string Filename { get; set; }
        public ActionEnum Action { get; set; }
    }
    public class ResponseJobHistoyDTO
    {
        public List<JobHistoryDTO> JobHistory { get; set; }
        public int TotalRecord { get; set; }
    }
}
