using Encryptor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryptor.Functions
{
    public class HardCryptoCore : Functions.Function
    {
        protected string Path;
        protected byte[] Password1;
        protected byte[] Password2;
        protected int Difficulty;

        /// <summary>
        /// Unify password1 and password2 in new byte array
        /// </summary>
        /// <returns></returns>
        protected byte[] MixPasswords()
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
        protected int GenerateNumericPassword()
        {
            return MixPasswords().OrderBy(b => b).Last();
        }

        /// <summary>
        /// Get rounds to hash be computed
        /// </summary>
        /// <param name="currentRound"></param>
        /// <returns></returns>
        protected long GenerateHashRounds(long currentRound) 
        {
            return currentRound * 1000;
        }

        /// <summary>
        /// Create private key to current AES round
        /// </summary>
        /// <param name="currentRound"></param>
        /// <returns></returns>
        protected byte[] GeneratePrivateKey(long currentRound)
        {
            var salt = MixPasswords();

            var saltedKey = new List<byte>();
            saltedKey.AddRange(Password1);
            saltedKey.AddRange(salt);

            var rounds = GenerateHashRounds(currentRound);
            var hashed = saltedKey.ToArray().SHA512(rounds);
            var finalHash = hashed.SHA256(rounds);

            return finalHash;
        }

        /// <summary>
        /// Create Initial Vector to current AES round
        /// </summary>
        /// <param name="currentRound"></param>
        /// <returns></returns>
        protected byte[] GenerateInitialVector(long currentRound)
        {
            var rounds = GenerateHashRounds(currentRound);
            var hash = Password2.SHA512(rounds);
            var finalHash = hash.SHA256(rounds).MD5(rounds);

            return finalHash;
        }

        /// <summary>
        /// Get main encryption rounds
        /// </summary>
        /// <returns></returns>
        protected long GenerateRounds() 
        {
            return GenerateNumericPassword() * Difficulty;
        }

        public override string GetHelp()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        public override void Execute(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
