using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class SLGGroup
    {
        [Key]
        public Guid Id { get; set; }
        public string InitialStatus {  get; set; }
        public string StatusName {  get; set; }
        public string StatusSequence { get; set; }
        public string RelevantExperience { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
