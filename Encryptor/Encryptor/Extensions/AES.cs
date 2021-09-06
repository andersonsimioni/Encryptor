using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Encryptor.Extensions
{
    public static class AES
    {
        /// <summary>
        /// Create random AES initial vector
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateIV() 
        {
            var aes = Aes.Create();
            aes.GenerateIV();

            return aes.IV;
        }

        /// <summary>
        /// Encode byte data into cipher data
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="initialVector"></param>
        /// <returns></returns>
        public static byte[] EncodeAES256CBC(this byte[] value, byte[] key, byte[] initialVector, long rounds = 1)
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.KeySize = 256;
            aes.Key = key;
            aes.IV = initialVector;

            var core = aes.CreateEncryptor(aes.Key, aes.IV);

            byte[] cipher;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, core, CryptoStreamMode.Write))
                {
                    cs.Write(value, 0, value.Length);
                    cs.FlushFinalBlock();
                }
                cipher = ms.ToArray();
            }

            return rounds == 0 ? cipher : cipher.EncodeAES256CBC(key, initialVector, rounds-1);
        }

        /// <summary>
        /// Compute Aes decryption
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="initialVector"></param>
        /// <returns></returns>
        public static byte[] DecodeAES256CBC(this byte[] value, byte[] key, byte[] initialVector, long rounds = 1)
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.KeySize = 256;
            aes.Key = key;
            aes.IV = initialVector;

            var core = aes.CreateDecryptor();

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, core, CryptoStreamMode.Write))
                {
                    cs.Write(value, 0, value.Length);
                    cs.FlushFinalBlock();
                }
                return rounds == 0 ? ms.ToArray() : ms.ToArray().DecodeAES256CBC(key, initialVector, rounds-1);
            }
        }
    }
}
