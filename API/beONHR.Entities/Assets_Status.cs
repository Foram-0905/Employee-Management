using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beONHR.Entities
{
    public class Assets_Status
    {
        [Key]
        public Guid Id { get; set; }


        public string Status { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}