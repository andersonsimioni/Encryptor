using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encryptor.Extensions;

namespace Encryptor.Functions
{
    public class DecodeHardEncoding : HardCryptoCore
    {
        public override string GetHelp()
        {
            return "DecodeHardEnconding \"file path\" \"first password\" \"second password\" \"numeric password\" \"difficulty number\"(optional)";
        }

        public override string GetName()
        {
            return "DecodeHardEnconding";
        }

        public override void Execute(string[] args)
        {
            if (args.Length < 4)
                throw new Exception("Invalid arguments");

            var functionaNameId = args[0];
            Path = args[1];
            Password1 = args[2].GetUTF8ByteArray();
            Password2 = args[3].GetUTF8ByteArray();

            Difficulty = 1;
            if (args.Length > 4)
                if (args[4].ToInt32() > 0)
                    Difficulty = args[4].ToInt32();

            if (File.Exists(Path) == false)
                throw new Exception("File not found");

            var fileDTO = DTO.FileDecoder.Load(Path);

            ReportInitFunction();

            var difficultedRounds = GenerateRounds();
            for (long currentRound = difficultedRounds; currentRound >= 1; currentRound--)
            {
                var privateKey = GeneratePrivateKey(currentRound);
                var initialVector = GenerateInitialVector(currentRound);
                fileDTO.Content = fileDTO.Content.DecodeAES256CBC(privateKey, initialVector, currentRound);
                ReportProgress(difficultedRounds-currentRound, difficultedRounds);
            }

            fileDTO.Save(Path);
            ReportFinishFunction();
        }
    }
}
