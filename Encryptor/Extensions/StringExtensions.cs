using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Convert normal text to hex string format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TextToHexString(this string value, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            return BitConverter.ToString(encoding.GetBytes(value)).Replace("-", string.Empty);
        }

        /// <summary>
        /// Convert hex string to byte array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(this string value)
        {
            return Enumerable.Range(0, value.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(value.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// Encode string to add in Web URL
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UrlEncode(this string value)
        {
            return System.Net.WebUtility.UrlEncode(value);
        }

        /// <summary>
        /// Return URL value decoded
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UrlDecode(this string value)
        {
            return System.Net.WebUtility.UrlDecode(value);
        }

        /// <summary>
        /// Convert string to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Convert string xml format to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeXML<T>(this string value)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(value))
            {
                var result = (T)serializer.Deserialize(reader);

                return result;
            }
        }

        /// <summary>
        /// Clear specied chars from string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string ClearChars(this string value, params string[] chars)
        {
            var newVal = value;
            foreach (var c in chars)
                value = value.Replace(c, "");

            return newVal;
        }

        /// <summary>
        /// Convert string to UpperTrim format like HELLOWORLD
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UpperTrim(this string value)
        {
            return value.Replace(" ", "").ToUpper();
        }

        /// <summary>
        /// Remove empty spaces of string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Minify(this string value)
        {
            return value.Replace(" ", "");
        }

        /// <summary>
        /// Convert string to byte array of UTF8 enconding byte default if is null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string value, Encoding encoding = null)
        {
            try
            {
                if (encoding == null)
                    encoding = Encoding.UTF8;
                return encoding.GetBytes(value);
            }
            catch
            {
                throw new Exception("Error on convert string to byte array");
            }
        }

        /// <summary>
        /// Convert string to int32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Int32(this string value) 
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on convert string to int32");
            }
        }

        /// <summary>
        /// Convert string to ulong
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ulong Ulong(this string value)
        {
            try
            {
                return ulong.Parse(value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on convert string to ulong");
            }
        }
    }
}
