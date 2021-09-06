using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Encryptor.Extensions
{
    public static class Hash
    {
        /// <summary>
        /// Compute MD5 hash from byte array with rounds number
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rounds"></param>
        /// <returns></returns>
        public static byte[] MD5(this byte[] value, long rounds = 1) 
        {
            var algo = HashAlgorithm.Create("MD5");
            var data = value;

            for (long i = 0; i < rounds; i++)
                data = algo.ComputeHash(data);

            return data;
        }

        /// <summary>
        /// Compute SHA128 with rounds
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rounds"></param>
        /// <returns></returns>
        public static byte[] SHA128(this byte[] value, long rounds = 1) 
        {
            var core = SHA1.Create("SHA128");
            byte[] hash = value;

            for (long i = 0; i < rounds; i++)
                hash = core.ComputeHash(hash);

            return hash;
        }

        /// <summary>
        /// Compute SHA256 hash from byte array with rounds number
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rounds"></param>
        /// <returns></returns>
        public static byte[] SHA256(this byte[] value, long rounds = 1) 
        {
            var algo = HashAlgorithm.Create("SHA256");
            var data = value;

            for (long i = 0; i < rounds; i++)
                data = algo.ComputeHash(data);

            return data;
        }

        /// <summary>
        /// Compute SHA512 hash from byte array with rounds number
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rounds"></param>
        /// <returns></returns>
        public static byte[] SHA512(this byte[] value, long rounds = 1)
        {
            var algo = HashAlgorithm.Create("SHA512");
            var data = value;

            for (long i = 0; i < rounds; i++)
                data = algo.ComputeHash(data);

            return data;
        }

        /// <summary>
        /// Encode all hash algorithms byte name and with rounds
        /// </summary>
        /// <param name="value"></param>
        /// <param name="hashName"></param>
        /// <param name="rounds"></param>
        /// <returns></returns>
        public static byte[] EncodeHash(this byte[] value, string hashName, long rounds = 1) 
        {
            var algo = HashAlgorithm.Create(hashName);
            var data = value;

            for (long i = 0; i < rounds; i++)
                data = algo.ComputeHash(data);

            return data;
        }
    }
}
