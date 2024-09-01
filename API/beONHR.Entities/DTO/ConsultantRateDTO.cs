using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class ConsultantRateDTO
    {
        public Guid id { get; set; }
        [JsonPropertyName("Currency")]
        public Guid Currency { get; set; } //foreign key
        [JsonPropertyName("PriceperDaynet")]
        public float PricePerDayNet { get; set; }
        [JsonPropertyName("PriceperHournet")]
        public float PricePerHourNet { get; set; }
        public Guid EmployeeId { get; set; }
        public ActionEnum Action { get; set; }


    }
}
