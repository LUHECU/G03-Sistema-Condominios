using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace G03_Sistema_Condominios.Models
{
    public class Login
    {
        
        
        public string Email { get; set; }
         public string Password { get; set; }
    }

    public class UserModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
    }
}