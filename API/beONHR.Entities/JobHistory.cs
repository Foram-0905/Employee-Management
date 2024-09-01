using beONHR.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class JobHistory
    {
        [Key]
        public Guid Id { get; set; }
        public string CompanyName {  get; set; }
        public string PositionHeld {  get; set; }
        public Guid EmploymentType { get; set; }
        public int ZipCode { get; set; }
        public Guid City { get; set;} //forignkey
        public Guid Country { get; set;} //forignkey
        public Guid StateId { get; set;} //forignkey
        public DateOnly StartDate{ get; set;} 
        public DateOnly EndDate{ get; set;} 
        public string Document {  get; set;}
        public string LeavingReason { get; set;}
        public Guid EmployeeId { get; set; } //forignkey
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("EmploymentType")]
        public virtual TypeofEmployment? EmployeeType { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [ForeignKey("City")]
        public virtual City? cityId { get; set; }

        [ForeignKey("Country")]
        public virtual Country? countryId{ get; set; }


        [ForeignKey("StateId")]
        public virtual State? State { get; set; }

    }
}
