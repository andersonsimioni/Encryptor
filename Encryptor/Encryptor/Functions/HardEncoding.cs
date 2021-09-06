using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encryptor.Extensions;
using System.IO;
using System.Xml;

namespace Encryptor.Functions
{
    public class HardEncoding : HardCryptoCore
    {
        public override string GetHelp()
        {
            return "HardEnconding \"file path\" \"first password\" \"second password\" \"numeric password\" \"difficulty number\"(optional)";
        }

        public override string GetName()
        {
            return "HardEnconding";
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

            var fileDTO = DTO.FileEncoder.Load(Path);

            ReportInitFunction();

            var difficultedRounds = GenerateRounds();
            for (long currentRound = 1; currentRound <= difficultedRounds; currentRound++)
            {
                var privateKey = GeneratePrivateKey(currentRound);
                var initialVector = GenerateInitialVector(currentRound);
                fileDTO.Content = fileDTO.Content.EncodeAES256CBC(privateKey, initialVector, currentRound);
                ReportProgress(currentRound, difficultedRounds);
            }

            fileDTO.Save(Path);
            ReportFinishFunction();
        }
    }
}
