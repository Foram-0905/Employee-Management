using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class EmployeeEduction
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EductionLevel { get; set; } //Foriegnkey
        public string? SubjectOfStudy { get; set;}
        public string? InstitutionName { get; set; }
        public Guid City { get; set; } //foriengkey
        public Guid Country { get; set; }//foriengkey
        public DateOnly CompletionDate { get; set; }
        public string? CertificateFile{ get; set; }
        public string? Anabin { get; set; }
        public Guid EmployeeId { get; set; } //forigenkey
        public bool IsDeleated { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [ForeignKey("EductionLevel")]
        public virtual EductionLevel? eductionLevelId { get; set; }

        [ForeignKey("City")]
        public virtual City? cityId { get; set; }  

        [ForeignKey("Country")]
        public virtual Country? countryId { get; set; }
    }
}
