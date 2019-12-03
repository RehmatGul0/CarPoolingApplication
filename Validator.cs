using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarPoolingApp
{
    public static class Validator
    {
        private static EmailAddressAttribute emailValidate = new EmailAddressAttribute();

        public static bool validateEmail(string email)
        {
            return emailValidate.IsValid(email);
        }
    }
}
