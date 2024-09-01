using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class SalaryDTO
    {
        public Guid Id { get; set; }
        public Guid SalaryType { get; set; }//foreign key
        public Guid TransactionType { get; set; } //foreign key 
        public double Amount { get; set; }
        public Guid Currency { get; set; }//foreign key 
        public DateOnly SalaryStartDate { get; set; }
        public DateOnly SalaryEndDate { get; set; }
        
        public Guid EmployeeId { get; set; } // foreign key

        public ActionEnum Action { get; set; }
    }
    public class ResponseSalaryDto
    {
        public List<Salary> salary { get; set; }
        public int TotalRecord { get; set; }


    }
}
