using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beONHR.Entities.DTO.Enum;

namespace beONHR.Entities.DTO
{
    public class NotificationDTO
    {

        public Guid NotificationID { get; set; }
        public Guid EmployeeId { get; set; } // Assuming EmployeeId is the foreign key referencing the user
        public string NotificationText { get; set; }
        public string NotificationType { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
      //  public ActionEnum Action { get; set; }


    }


}
