using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceAPI
{
    public class Utility
    {
        public static string GetClientIpAddress()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }
    }
}