using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class EmployeenStatus
    {
        [Key]
        public Guid Id { get; set; }
        public string employeenstatus { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
