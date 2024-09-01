using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beONHR.Entities
{
    public class taxclass
    {
        [Key]
        public Guid Id { get; set; }


        public string Tax_Class { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}