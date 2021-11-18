using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryptor.EncryptionMethods
{
    public abstract class LongMethod
    {
        public byte[] Password1 { get; set; }
        public byte[] Password2 { get; set; }
        public ulong Difficulty { get; set; }

        /// <summary>
        /// Unify password1 and password2 in new byte array
        /// </summary>
        /// <returns></returns>
        public byte[] MixPasswords()
        {
            var mixed = new List<byte>();

            var biggerLen = Password1.Length > Password2.Length ? Password1.Length : Password2.Length;
            for (long i = 0; i < biggerLen; i++)
            {
                if (i < Password1.Length)
                    mixed.Add(Password1[i]);
                if (i < Password2.Length)
                    mixed.Add(Password2[i]);
            }

            return mixed.ToArray();
        }

        /// <summary>
        /// Get bigger byte of mixed password byte array 0x0-0xFF
        /// </summary>
        /// <returns></returns>
        public ulong GenerateNumericPassword()
        {
            return (ulong)MixPasswords().OrderBy(b => b).Last();
        }

        /// <summary>
        /// Get rounds to hash be computed
        /// </summary>
        /// <param name="currentRound"></param>
        /// <returns></returns>
        protected ulong GenerateHashRounds(ulong currentRound)
        {
            return currentRound * 1000;
        }

        /// <summary>
        /// Create private key to current AES round
        /// </summary>
        /// <param name="currentRound"></param>
        /// <returns></returns>
        public byte[] GeneratePrivateKey(ulong currentRound)
        {
            var salt = MixPasswords();

            var saltedKey = new List<byte>();
            saltedKey.AddRange(Password1);
            saltedKey.AddRange(salt);

            var rounds = GenerateHashRounds(currentRound);
            var hashed = saltedKey.ToArray().EncodeSHA512((ulong)rounds);
            var finalHash = hashed.EncodeSHA256((ulong)rounds);

            return finalHash;
        }

        /// <summary>
        /// Create Initial Vector to current AES round
        /// </summary>
        /// <param name="currentRound"></param>
        /// <returns></returns>
        public byte[] GenerateInitialVector(ulong currentRound)
        {
            var rounds = GenerateHashRounds(currentRound);
            var hash = Password2.EncodeSHA512(rounds);
            var finalHash = hash.EncodeSHA256(rounds).EncodeMD5(rounds);

            return finalHash;
        }

        /// <summary>
        /// Get main encryption rounds
        /// </summary>
        /// <returns></returns>
        public ulong GenerateRounds()
        {
            return GenerateNumericPassword() * Difficulty;
        }
    }
}
