using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class LeaveType
    {
        [Key]
        public Guid Id { get; set; }
        public string TypeName { get; set; }
        public Guid CategoryName { get; set; } //forignkey
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("CategoryName")]
        public virtual LeaveCategory? LeaveCategory { get; set; }


    }
}
