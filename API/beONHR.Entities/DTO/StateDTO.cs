using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class StateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CountryId { get; set; }
        public ActionEnum Action { get; set; }
    }
    public class ResponseStateDto
    {
        public List<State> state { get; set; }
        public int TotalRecord { get; set; }


    }
}
