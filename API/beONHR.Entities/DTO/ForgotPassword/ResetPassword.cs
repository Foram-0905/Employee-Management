using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO.ForgotPassword
{
    public class ResetPassword
    {

        public string Token { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }



    }
}
