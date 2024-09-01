using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class BonusDTO
    {
        public Guid Id { get; set; }
        public bool Entitlement { get; set; }
        public DateOnly StartingDate { get; set; }
        public DateOnly EndingDate { get; set; }
        public string Bonusamount { get; set; }
        public Guid SalaryType { get; set; }
        public Guid EmployeeId { get; set; } //forignkey
        public ActionEnum Action { get; set; }
    }
    public class ResponseBonussDTO
    {
        public List<Bonus> Bonus { get; set; }
        public int TotalRecord { get; set; }
    }
}