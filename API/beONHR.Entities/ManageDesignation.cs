using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class ManageDesignation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid InitialStatus {  get; set; } 
        public string Designation {  get; set; }
        public string DisplaySequence {  get; set; }
        public string ShortWord {  get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("InitialStatus")]
        public virtual SLGGroup? SLGGroup{ get; set; }

    }


}
