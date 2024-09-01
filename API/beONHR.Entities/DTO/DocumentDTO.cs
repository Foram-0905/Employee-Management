using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class DocumentDTO
    {
        public Guid Id { get; set; }
        public string PersonalDataProtection { get; set; }
        public string ConfidentialityAgreement { get; set; }
        public string Other { get; set; }
        public Guid EmployeeId { get; set; } //forignkey
        public ActionEnum Action { get; set; }

    }
}
