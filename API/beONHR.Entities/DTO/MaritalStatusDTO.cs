using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class MaritalStatusDTO
    {
        public Guid Id { get; set; }
        public string maritalstatus { get; set; }
        public ActionEnum Action { get; set; }
    }
}
