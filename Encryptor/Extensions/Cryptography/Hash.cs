using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class Hash
    {
        /// <summary>
        /// Encode byte data to hash of MD5,
        /// WARNING, This hash is no longer secure
        /// </summary>
        /// <param name="value"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static byte[] EncodeMD5(this byte[] value, ulong salt = 1)
        {
            try
            {
                if (value == null)
                    throw new Exception("value is null or empty");
                if (salt == 0)
                    throw new Exception("Salt cannot be 0");

                var hashManager = MD5.Create();
                var hash = hashManager.ComputeHash(value);
                for (ulong i = 1; i < salt; i++)
                    hash = hashManager.ComputeHash(value);

                return hash;
            }
            catch
            {
                throw new Exception($"MD5 encoding error");
            }
        }

        /// <summary>
        /// Encode byte data to hash of SHA1,
        /// WARNING, This hash is no longer secure
        /// </summary>
        /// <param name="value"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static byte[] EncodeSHA1(this byte[] value, ulong salt = 1)
        {
            try
            {
                if (value == null)
                    throw new Exception("value is null or empty");
                if (salt == 0)
                    throw new Exception("Salt cannot be 0");

                var hashManager = SHA1.Create();
                var hash = hashManager.ComputeHash(value);
                for (ulong i = 1; i < salt; i++)
                    hash = hashManager.ComputeHash(value);

                return hash;
            }
            catch
            {
                throw new Exception($"SHA1 encoding error");
            }
        }

        /// <summary>
        /// Encode byte data to hash of SHA256
        /// </summary>
        /// <param name="value"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static byte[] EncodeSHA384(this byte[] value, ulong salt = 1)
        {
            try
            {
                if (value == null)
                    throw new Exception("value is null or empty");
                if (salt == 0)
                    throw new Exception("Salt cannot be 0");

                var hashManager = SHA384.Create();
                var hash = hashManager.ComputeHash(value);
                for (ulong i = 1; i < salt; i++)
                    hash = hashManager.ComputeHash(value);

                return hash;
            }
            catch
            {
                throw new Exception($"SHA384 encoding error");
            }
        }

        /// <summary>
        /// Encode byte data to hash of SHA256
        /// </summary>
        /// <param name="value"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static byte[] EncodeSHA256(this byte[] value, ulong salt = 1)
        {
            try
            {
                if (value == null)
                    throw new Exception("value is null or empty");
                if (salt == 0)
                    throw new Exception("Salt cannot be 0");

                var hashManager = SHA256.Create();
                var hash = hashManager.ComputeHash(value);
                for (ulong i = 1; i < salt; i++)
                    hash = hashManager.ComputeHash(value);

                return hash;
            }
            catch
            {
                throw new Exception($"SHA256 encoding error");
            }
        }

        /// <summary>
        /// Encode byte data to hash of SHA512
        /// </summary>
        /// <param name="value"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static byte[] EncodeSHA512(this byte[] value, ulong salt = 1)
        {
            try
            {
                if (value == null)
                    throw new Exception("value is null or empty");
                if (salt == 0)
                    throw new Exception("Salt cannot be 0");

                var hashManager = new SHA512Managed();
                var hash = hashManager.ComputeHash(value);
                for (ulong i = 1; i < salt; i++)
                    hash = hashManager.ComputeHash(value);

                return hash;
            }
            catch
            {
                throw new Exception($"SHA512 encoding error");
            }
        }
    }
}
