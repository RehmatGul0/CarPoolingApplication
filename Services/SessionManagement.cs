using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CarPoolingApp.Services
{
    public class SessionManagement
    {
        public string getSessionID(string email,string salt)
        {
            using (SHA256 _SHA256 = SHA256.Create())
            {
                byte[] token = Encoding.ASCII.GetBytes(email+salt+DateTime.Now+DateTime.Now.ToShortTimeString());
                return Convert.ToBase64String(_SHA256.ComputeHash(token));
            }
        }
    }
}
