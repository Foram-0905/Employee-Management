using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid NotificationID { get; set; }

        public Guid EmployeeId { get; set; } // Assuming EmployeeId is the foreign key referencing the user

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; } // Navigation property for the related employee

        [Required]
        public string NotificationText { get; set; }

        [Required]
        [StringLength(50)]
        public string NotificationType { get; set; }

        public bool IsRead { get; set; } = false;

        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;



    }
}
