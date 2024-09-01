using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace beONHR.Entities
{
    public class DocumentList
    {
        [Key]
        public Guid Id { get; set; }
        public string TabName { get; set; }
        public string Modulename { get; set; }
        public string FileName { get; set; }
        public string Documents { get; set; }
        public Guid EmployeeId { get; set; } //forignkey
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

    }
}
