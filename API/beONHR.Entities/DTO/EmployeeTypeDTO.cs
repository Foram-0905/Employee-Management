﻿using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class EmployeeTypeDTO
    {
        public Guid Id { get; set; }
        public string EmployeeTypeName { get; set; }
        public ActionEnum Action { get; set; }
    }
}