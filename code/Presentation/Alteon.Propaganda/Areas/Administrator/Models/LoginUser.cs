using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alteon.Propaganda.Areas.Administrator.Models
{
    public class LoginUser
    {
        public string Mobile { get; set; }
        public string Password { get; set; }
    }

    public class CookieName
    {
        public const string loginUserCookieName = "loginUser";
    }
}