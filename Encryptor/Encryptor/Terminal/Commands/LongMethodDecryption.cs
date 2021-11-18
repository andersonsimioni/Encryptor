using Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Encryptor.Terminal.Commands
{
    public class LongMethodDecryption : EncryptionMethods.LongMethod, ITerminalCommand
    {
        public ArgumentList Arguments { get; set; }

        public string GetDescription()
        {
            return "Use AES 256 bits CBC encryption to encode file or text with 2 passwords";
        }

        public string GetHelp()
        {
            return
                $"\n-d, --data: data to decrypt, can be file or text" +
                $"\n-p1, --password1: first password of encryption, can be path or text" +
                $"\n-p2, --password2: second password of encryption, can be path or text" +
                $"\n-di, --difficulty: encryption level, is number, max recommended is 2";
        }

        public string GetName()
        {
            return "LongMethodDecryption".ToUpper();
        }

        public byte[] GetByteArrayOfData()
        {
            var data = (this.Arguments?.GetArgumentValue("d", "data") ?? "");
            if (File.Exists(data))
                return File.ReadAllBytes(data);
            else
                return data.ToByteArray();
        }

        public byte[] Execute(ArgumentList arguments)
        {
            this.Arguments = arguments;

            Password1 = (this.Arguments?.GetArgumentValue("p1", "password1") ?? "").ToByteArray();
            Password2 = (this.Arguments?.GetArgumentValue("p2", "password2") ?? "").ToByteArray();
            Difficulty = (this.Arguments?.GetArgumentValue("di", "difficulty") ?? "1").Ulong();

            var difficultedRounds = GenerateRounds();
            var decryptedContent = GetByteArrayOfData();
            for (ulong currentRound = difficultedRounds; currentRound >= 1; currentRound--)
            {
                var privateKey = GeneratePrivateKey(currentRound);
                var initialVector = GenerateInitialVector(currentRound);
                decryptedContent = decryptedContent.DecodeAES256CBCRandomInfo(privateKey, initialVector, currentRound);
                ITerminalCommand.ReportProgress(arguments, difficultedRounds - currentRound, difficultedRounds);
            }

            ITerminalCommand.ReportFinishFunction(arguments);

            return decryptedContent;
        }
    }
}
