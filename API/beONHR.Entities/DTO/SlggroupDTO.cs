using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class SlggroupDto
    {
        public Guid Id { get; set; }
        public string InitialStatus { get; set; }
        public string StatusName { get; set; }
        public string StatusSequence { get; set; }
        public string RelevantExperience { get; set; }
        public ActionEnum Action { get; set; }
    }

    public class ResponseSlggroupDto
    {
        public List<SLGGroup> slggroups { get; set; }
        public int TotalRecord { get; set; }
    }
}
