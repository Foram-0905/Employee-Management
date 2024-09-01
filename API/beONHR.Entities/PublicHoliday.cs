using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class PublicHoliday
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Country { get; set; }//forignkey
        public Guid State { get; set; }//forignkey
        public string HolidayName { get; set; }
        public DateOnly HolidayDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("State")]
        public virtual State? stateId { get; set; }

        [ForeignKey("Country")]
        public virtual Country? countryId { get; set; }

    }
}
