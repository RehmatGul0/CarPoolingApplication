using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPoolingApp.Services
{
    public static class ExtensionMethods
    {
        public static object GetValObj(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj);
        }
    }
}
