using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class CountryDTO
    {
        public Guid Id { get; set; }
        public string CountryName { get; set; }
        public ActionEnum Action { get; set; }
    }
    public class ResponseCountryDto
    {
        public List<Country> country { get; set; }
        public int TotalRecord { get; set; }


    }
}
