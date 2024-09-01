using beONHR.Entities.DTO.Enum;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class PublicHolidayDTO
    {
        public Guid Id { get; set; }
        public Guid Country { get; set; }//forignkey
        public Guid State { get; set; }//forignkey
        public string HolidayName { get; set; }
        public DateOnly HolidayDate { get; set; }
        public ActionEnum Action { get; set; }


    }
    public class ResponsePublicHolidayDto
    {
        public List<PublicHoliday> publicHoliday { get; set; }
        public int TotalRecord { get; set; }


    }
}