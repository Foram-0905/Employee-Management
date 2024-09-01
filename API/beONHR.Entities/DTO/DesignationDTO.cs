using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class DesignationDto
    {
        public Guid Id { get; set; }
        public Guid InitialStatus { get; set; }
        public string Designation { get; set; }
        public string DisplaySequence { get; set; }
        public string ShortWord { get; set; }
        public ActionEnum Action { get; set; }

    }

    public class ResponseDesignationDto
    {
        public List<ManageDesignation> designation { get; set; }
        public int TotalRecord { get; set; }


    }
}
