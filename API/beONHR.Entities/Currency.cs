using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class Currency
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Country { get; set; }
        public string ShortWord { get; set; }
        public string Symbol { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Country")]
        public virtual Country? countryId { get; set; }
    }
}
