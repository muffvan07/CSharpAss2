using System;
using System.Text;

namespace CSharpAssignment2.Models
{
    public class EncryptPassword
    {
        public static string Encrypt(string paasWord)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(paasWord)));
        }
    }
}