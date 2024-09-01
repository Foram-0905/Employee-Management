using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class LanguageLevelDTO
    {
        public Guid Id { get; set; }
        public string Level { get; set; }
        public ActionEnum Action { get; set; }
    }
    public class ResponseLanguageLevelDTO
    {
        public List<LanguageLevel> languagelevel { get; set; }
        public int TotalRecord { get; set; }


    }
}