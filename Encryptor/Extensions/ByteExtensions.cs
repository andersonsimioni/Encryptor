using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class ByteExtensions
    {
        /// <summary>
        /// Create random information based on datetime
        /// </summary>
        /// <returns></returns>
        private static byte[] GetRandomInformationByDatetime() 
        {
            var randomString = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:fffffff");
            var randomBytes = randomString.ToByteArray().EncodeSHA384();

            return randomBytes;
        }

        /// <summary>
        /// Return string data from byte array of UTF8 Encoding by default is null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertToString(this byte[] value, Encoding encoding = null)
        {
            try
            {
                if (value.Length == 0)
                    throw new Exception("Empty array");

                if (encoding == null)
                    encoding = Encoding.UTF8;

                return encoding.GetString(value);
            }
            catch
            {
                throw new Exception("Byte string parse system error");
            }
        }

        /// <summary>
        /// Convert to HEX string format
        /// Return string data from byte array using 
        /// .ToString("X2") method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertToStringX2(this byte[] value)
        {
            try
            {
                return string.Concat(value.Select(b => b.ToString("x2")));
            }
            catch (Exception ex)
            {
                throw new Exception("Error on convert byte to X2 string format");
            }
        }

        /// <summary>
        /// Convert string to default hex string format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertToHexString(this byte[] value) 
        {
            try
            {
                return value.ConvertToStringX2();
            }
            catch (Exception ex)
            {
                throw new Exception("Error on convert to hex string");
            }
        }

        /// <summary>
        /// Convert byte data to combined random info based on datetime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ConvertToRandomFormat(this byte[] value) 
        {
            try
            {
                var randomInfo = GetRandomInformationByDatetime();
                var combined = new byte[randomInfo.Length + value.Length];

                Array.Copy(value, combined, value.Length);
                Array.Copy(randomInfo, 0, combined, value.Length, randomInfo.Length);

                return combined;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on convert to random format");
            }
        }

        /// <summary>
        /// Convert byte data from combined random info based on datetime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ConvertFromRandomFormat(this byte[] value) 
        {
            try
            {
                var sizeBase = GetRandomInformationByDatetime().Length;
                var originalInfo = new byte[value.Length - sizeBase];

                Array.Copy(value, originalInfo, originalInfo.Length);

                return originalInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on convert from random format");
            }
        }
    }
}
