﻿using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class EductionLevelDTO
    {
        public Guid Id { get; set; }
        public string Level { get; set; }
        public ActionEnum Action { get; set; }
    }
    public class ResponseEductionLevelDTO
    {
        public List<EductionLevel> eductionlevel { get; set; }
        public int TotalRecord { get; set; }


    }
}