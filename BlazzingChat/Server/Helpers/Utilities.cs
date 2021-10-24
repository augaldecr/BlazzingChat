using System;
using System.Security.Cryptography;
using System.Text;

namespace BlazzingChat.Server.Helpers
{
    public class Utilities
    {
        public static string Encrypt(string password)
        {
            var provider = MD5.Create();
            string salt = "fsepo'093'032w3487987+65sfes90e3";
            byte[] bytes = provider.ComputeHash(Encoding.UTF8.GetBytes(salt + password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
