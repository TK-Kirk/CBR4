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
            string ip = HttpContext.Current.Request.UserHostAddress;

            if (ip == "127.0.0.1")
            {
                return "68.225.172.87";
            }
            return ip;
        }
    }
}