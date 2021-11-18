using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class Random
    {
        private static System.Random random = new System.Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Generate a random string
        /// </summary>
        /// <param name="len"></param>
        /// <param name="useChars"></param>
        /// <param name="useNumbers"></param>
        /// <param name="useSpecials"></param>
        /// <returns></returns>
        public static string GenerateString(int len, bool useChars = true, bool useNumbers = true, bool useSpecials = false)
        {
            try
            {
                var chars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
                var numbers = "0123456789";
                var specialChars = "çÇ!@'#$%¨&*()_-+=/?|\\[]{}´`;:.<,>";
                var globalDic = (useChars ? chars : "") + (useNumbers ? numbers : "") + (useSpecials ? specialChars : "");

                var randomWord = new StringBuilder();

                for (int i = 0; i < len; i++)
                    randomWord.Append(globalDic[random.Next(0, globalDic.Length)]);

                return randomWord.ToString();
            }
            catch
            {
                throw;
            }
        }
    }
}
