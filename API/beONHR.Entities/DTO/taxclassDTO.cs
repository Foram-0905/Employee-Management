using beONHR.Entities.DTO.Enum;
using System;

namespace beONHR.Entities.DTO
{
    public class taxclassDTO
    {
        public Guid Id { get; set; }
        public string Tax_Class { get; set; }

        public ActionEnum Action { get; set; }
    }

}