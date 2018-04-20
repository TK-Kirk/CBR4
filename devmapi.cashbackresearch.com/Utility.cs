using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace devmapi.cashbackresearch.com
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

        public static string HashHmac(string message, string secret)
        {
            Encoding encoding = Encoding.UTF8;
            using (HMACSHA512 hmac = new HMACSHA512(encoding.GetBytes(secret)))
            {
                var msg = encoding.GetBytes(message);
                var hash = hmac.ComputeHash(msg);
                return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
            }
        }


        public static string HashHmacMD5(string message, string key)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);
            HMACMD5 hmacmd5 = new HMACMD5(keyByte);
            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
            byte[] messageBytes = encoding.GetBytes(message);
            byte[] hashmessage = hmacmd5.ComputeHash(messageBytes);
            return  ByteToString(hashmessage);
        }
        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
    }
}