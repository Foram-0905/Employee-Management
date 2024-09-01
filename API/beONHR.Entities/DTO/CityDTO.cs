using beONHR.Entities.DTO.Enum;
using System;

namespace beONHR.Entities.DTO
{
    public class CityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CountryId { get; set; } // Assuming CountryId is a foreign key
        public Guid State { get; set; } // Assuming StateId is a foreign key
        public ActionEnum Action { get; set; }
    }
    public class ResponseCityDto
    {
        public List<City> city { get; set; }
        public int TotalRecord { get; set; }
    }
}
