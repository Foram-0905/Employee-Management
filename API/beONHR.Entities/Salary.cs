using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class Salary
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SalaryType { get; set; }//foreign key
        public Guid TransactionType { get; set; } //foreign key
        public double Amount { get; set; }
        public Guid Currency { get; set; }//foreign key 
        public DateOnly SalaryStartDate { get; set; }
        public DateOnly SalaryEndDate { get; set; }

        public Guid EmployeeId { get; set; } // foreign key
        [ForeignKey("SalaryType")]
        public virtual SalaryType? SalaryTypeName { get; set; }

        [ForeignKey("TransactionType")]
        public virtual TransactionType? TransactionTypeName { get; set; }

        [ForeignKey("Currency")]
        public virtual Currency? Currenccy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
    }


    public class ResponseSalary
    {
        public Guid Id { get; set; }
        public string SalaryType { get; set; }//foreign key
        public Guid TransactionType { get; set; } //foreign key
        public double Amount { get; set; }
        public Guid Currency { get; set; }//foreign key 
        public DateOnly SalaryStartDate { get; set; }
        public DateOnly SalaryEndDate { get; set; }
    }
}
