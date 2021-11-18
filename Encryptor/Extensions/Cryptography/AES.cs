using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class AES
    {
        /// <summary>
        /// this function use year and month information to
        /// create new combination key
        /// </summary>
        /// <param name="baseInfo"></param>
        /// <returns></returns>
        public static byte[] Generate256BitsKeyByDatetime(byte[] baseInfo) 
        {
            try
            {
                if (baseInfo == null)
                    throw new Exception("baseInfo cannot be null");
                if (baseInfo.Length == 0)
                    throw new Exception("baseInfo cannot be empty");

                var _base = new byte[baseInfo.Length];
                Array.Copy(baseInfo, _base, _base.Length);

                var datetimeInfo = (DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString()).ToByteArray().EncodeSHA256((ulong)DateTime.Now.Month);

                var combined = new byte[_base.Length + datetimeInfo.Length];
                Array.Copy(_base, 0, combined, 0, _base.Length);
                Array.Copy(datetimeInfo, 0, combined, _base.Length, datetimeInfo.Length);

                var combinedHash = combined.EncodeSHA256();

                return combinedHash;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on generate key by datetime");
            }
        }

        /// <summary>
        /// this function use year and month information to
        /// create new combination initial vector
        /// </summary>
        /// <param name="baseInfo"></param>
        /// <returns></returns>
        public static byte[] GenerateInitialVectorByDatetime(byte[] baseInfo)
        {
            try
            {
                if (baseInfo == null)
                    throw new Exception("baseInfo cannot be null");
                if (baseInfo.Length == 0)
                    throw new Exception("baseInfo cannot be empty");

                var _base = new byte[baseInfo.Length];
                Array.Copy(baseInfo, _base, _base.Length);

                var datetimeInfo = (DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString()).ToByteArray().EncodeMD5((ulong)DateTime.Now.Month);

                var combined = new byte[_base.Length + datetimeInfo.Length];
                Array.Copy(_base, 0, combined, 0, _base.Length);
                Array.Copy(datetimeInfo, 0, combined, _base.Length, datetimeInfo.Length);

                var combinedHash = combined.EncodeMD5();

                return combinedHash;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on generate key by datetime");
            }
        }

        /// <summary>
        /// Create new random 256 bits key
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateKey() 
        {
            var aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateKey();

            return aes.Key;
        }

        /// <summary>
        /// Create random AES initial vector
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateIV()
        {
            var aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateIV();

            return aes.IV;
        }

        /// <summary>
        /// Generate AES 256bits key from passphrase
        /// </summary>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        public static byte[] GenerateKey(byte[] passphrase) 
        {
            if ((passphrase?.Length ?? 0) == 0)
                throw new Exception("passphrase is null or empty");

            var interations = passphrase.Sum(b=>(int)b);
            var hash = passphrase.EncodeSHA256();
            for (int i = 1; i < interations; i++)
                hash = hash.OrderBy(b=>b).ToArray().EncodeSHA256();

            return hash;
        }

        /// <summary>
        /// Generate AES 128bits initial vector from passphrase
        /// </summary>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        public static byte[] GenerateIV(byte[] passphrase)
        {
            if ((passphrase?.Length ?? 0) == 0)
                throw new Exception("passphrase is null or empty");

            var interations = passphrase.Sum(b => (int)b);
            var hash = passphrase.EncodeSHA256();
            for (int i = 1; i < interations; i++)
                hash = hash.OrderBy(b => b).ToArray().EncodeSHA256();

            var md5 = hash.EncodeMD5();
            return md5;
        }

        /// <summary>
        /// Encode byte data into cipher data
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="initialVector"></param>
        /// <returns></returns>
        public static byte[] EncodeAES256CBC(this byte[] value, byte[] key, byte[] initialVector, ulong rounds = 1)
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

            return rounds == 0 ? cipher : cipher.EncodeAES256CBC(aes.Key, aes.IV, rounds - 1);
        }

        /// <summary>
        /// Compute Aes decryption
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="initialVector"></param>
        /// <returns></returns>
        public static byte[] DecodeAES256CBC(this byte[] value, byte[] key, byte[] initialVector, ulong rounds = 1)
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
                return rounds == 0 ? ms.ToArray() : ms.ToArray().DecodeAES256CBC(aes.Key, aes.IV, rounds - 1);
            }
        }
    
        /// <summary>
        /// Encode AES without Inivital Vector
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="rounds"></param>
        /// <returns></returns>
        public static byte[] EncodeAES256simple(this byte[] value, byte[] key, ulong rounds = 1) 
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.KeySize = 256;
            aes.Key = key;
            aes.IV = new byte[16];

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

            return rounds == 0 ? cipher : cipher.EncodeAES256CBC(aes.Key, aes.IV, rounds - 1);
        }

        /// <summary>
        /// Encode AES without Inivital Vector
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="rounds"></param>
        /// <returns></returns>
        public static byte[] DecodeAES256simple(this byte[] value, byte[] key, ulong rounds = 1)
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.KeySize = 256;
            aes.Key = key;
            aes.IV = new byte[16];

            var core = aes.CreateDecryptor(aes.Key, aes.IV);

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

            return rounds == 0 ? cipher : cipher.EncodeAES256CBC(aes.Key, aes.IV, rounds - 1);
        }



        /// <summary>
        /// Encode randonized byte data into cipher data
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="initialVector"></param>
        /// <returns></returns>
        public static byte[] EncodeAES256CBCRandomInfo(this byte[] value, byte[] key, byte[] initialVector, ulong rounds = 1)
        {
            try
            {
                var randonInfo = value.ConvertToRandomFormat();
                return randonInfo.EncodeAES256CBC(key, initialVector, rounds);
            }
            catch (Exception)
            {
                throw new Exception("Error on compute random AES");
            }
        }

        /// <summary>
        /// Decode randonized byte data into text data
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="initialVector"></param>
        /// <returns></returns>
        public static byte[] DecodeAES256CBCRandomInfo(this byte[] value, byte[] key, byte[] initialVector, ulong rounds = 1)
        {
            try
            {
                var decoded = value.DecodeAES256CBC(key, initialVector, rounds);
                var originalInfo = decoded.ConvertFromRandomFormat();

                return originalInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on compute random AES");
            }
        }

        /// <summary>
        /// Encode randonized AES without Inivital Vector
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="rounds"></param>
        /// <returns></returns>
        public static byte[] EncodeAES256simpleRandomInfo(this byte[] value, byte[] key, ulong rounds)
        {
            try
            {
                var randonInfo = value.ConvertToRandomFormat();
                return randonInfo.EncodeAES256simple(key, rounds);
            }
            catch (Exception)
            {
                throw new Exception("Error on compute AES");
            }
        }

        /// <summary>
        /// Encode randonized AES without Inivital Vector
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="rounds"></param>
        /// <returns></returns>
        public static byte[] DecodeAES256simpleRandomInfo(this byte[] value, byte[] key, ulong rounds)
        {
            try
            {
                var decoded = value.DecodeAES256simple(key, rounds);
                var originalInfo = decoded.ConvertFromRandomFormat();

                return originalInfo;
            }
            catch (Exception)
            {
                throw new Exception("Error on compute AES");
            }
        }
    }
}
