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
        public static byte[] EncodeAES256CBC(this byte[] value, byte[] key, byte[] initialVector)
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

            //byte[] combined = new byte[aes.IV.Length + cipher.Length];
            //Array.Copy(aes.IV, 0, combined, 0, aes.IV.Length);
            //Array.Copy(cipher, 0, combined, aes.IV.Length, cipher.Length);
            return cipher;
        }

        /// <summary>
        /// Compute Aes decryption
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="InitialVector"></param>
        /// <returns></returns>
        public static byte[] DecodeAES256CBC(this byte[] value, byte[] key, byte[] InitialVector)
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.KeySize = 256;
            aes.Key = key;
            aes.IV = InitialVector;

            //byte[] cipherText = new byte[combinedData.Length - iv.Length];
            //Array.Copy(combinedData, iv, iv.Length);
            //Array.Copy(combinedData, iv.Length, cipherText, 0, cipherText.Length);

            var core = aes.CreateDecryptor();

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, core, CryptoStreamMode.Write))
                {
                    cs.Write(value, 0, value.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }
    }
}
