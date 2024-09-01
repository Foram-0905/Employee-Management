using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace beONHR.Entities
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }
        public string PersonalDataProtection { get; set; }
        public string ConfidentialityAgreement { get; set; }
        public string Other { get; set; }
      
        public Guid EmployeeId { get; set; } //forignkey
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee{ get; set; }


    }
}
