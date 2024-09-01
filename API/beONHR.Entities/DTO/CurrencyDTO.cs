using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beONHR.Entities.DTO.Enum;

namespace beONHR.Entities.DTO
{
    public class CurrencyDTO
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Country { get; set; }
        public string ShortWord { get; set; }
        public string Symbol { get; set; }
        public ActionEnum Action { get; set; }

        [ForeignKey("Country")]
        public virtual Country? countryId { get; set; }
    }
    public class ResponseCurrencyDto
    {
        public List<Currency> currencies { get; set; }
        public int TotalRecord { get; set; }


    }
}
