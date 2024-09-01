using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class SalaryType
    {
        public Guid Id { get; set; }
        public string SalaryTypeName { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
