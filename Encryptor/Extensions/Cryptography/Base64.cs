using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Cryptography
{
    public static class Base64
    {
        /// <summary>
        /// Encode string to base64
        /// </summary>
        /// <param name="value"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncodeBase64(this byte[] value, ulong salt = 1)
        {
            if (value == null)
                throw new Exception("value is null");
            if (value.Length == 0)
                throw new Exception("value is empty");

            try
            {
                var enconded = Convert.ToBase64String(value);
                for (ulong i = 1; i < salt; i++)
                    enconded = Convert.ToBase64String(enconded.ToByteArray());

                return enconded;
            }
            catch
            {
                throw new Exception("Base64 enconde system error");
            }
        }

        /// <summary>
        /// Decode string from base64
        /// </summary>
        /// <param name="value"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string DecodeBase64(this string value, ulong salt = 1)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            try
            {
                var decoded = Convert.FromBase64String(value).ConvertToString();
                for (ulong i = 1; i < salt; i++)
                    decoded = Convert.FromBase64String(decoded).ConvertToString();

                return decoded;
            }
            catch
            {
                throw new Exception("Base64 decode system error");
            }
        }
    }
}
