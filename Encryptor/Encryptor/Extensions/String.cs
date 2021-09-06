using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Encryptor.Extensions
{
    public static class String
    {
        /// <summary>
        /// Convert byte array to UTF8 string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetUTF8String(this byte[] value) 
        {
            return Encoding.UTF8.GetString(value);
        }

        /// <summary>
        /// Return UTF8 enconding byte array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetUTF8ByteArray(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// Get hash pattern hex string like 0cc175b9c0f1b6a831c399e269772661
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetHexStringHashPattern(this byte[] value)
        {
            return string.Concat(value.Select(c => c.ToString("x2")));
        }

        /// <summary>
        /// Convert byte array to base64 string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetBase64(this byte[] value) 
        {
            return Convert.ToBase64String(value);
        }

        /// <summary>
        /// Get byte array from base64 string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetFromBase64(this string value) 
        {
            return Convert.FromBase64String(value);
        }
    
        /// <summary>
        /// Convert hex string to byte array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(this string value) 
        {
            var len = value.Length;
            var bytes = new byte[len / 2];

            for (int i = 0; i < len; i += 2)
                bytes[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);

            return bytes;
        }
    }
}
