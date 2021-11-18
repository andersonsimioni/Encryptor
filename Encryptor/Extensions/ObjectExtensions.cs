using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Convert object to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

    }
}
