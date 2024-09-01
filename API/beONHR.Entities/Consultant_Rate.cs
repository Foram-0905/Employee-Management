using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class Consultant_Rate
    {
        [Key]
        public Guid id { get; set; }
        public Guid Currency { get; set; } //foreign key
        public float PricePerDayNet { get; set; }
        public float PricePerHourNet { get;set; }
        public Guid EmployeeId { get; set; } // foreign key
        [ForeignKey("Currency")]
        public virtual Currency? Currenccy { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

    }
}
