using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL.ViewModels
{
    public class AdminLogin
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class AdminResponse
    {
        public string AccessToken { get; set; }
    }
    public class ForgetPassword
    {        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
    public class ChangePassword
    {              
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ChangePasswordReponse
    {
        public string Message { get; set; }    
    }

}