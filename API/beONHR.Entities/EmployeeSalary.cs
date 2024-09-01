using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class EmployeeSalary
    {
        [Key]
        public Guid Id { get; set; }
        public string SalaryType { get; set; }
        public string Amount { get; set; }
        public Guid Currency { get; set; }
        public DateOnly StartDate {  get; set; }
        public DateOnly EndDate {  get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("Currency")]
        public virtual Currency? currencyId { get; set; }      

     
    }
} 
