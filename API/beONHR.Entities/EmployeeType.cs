﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.User
{
    public class EmployeeType
    {
        public Guid Id { get; set; }
        public string EmployeeTypeName { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
