using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;

namespace Encryptor.Terminal.Commands
{
    public class LongMethodEncryption : EncryptionMethods.LongMethod, ITerminalCommand
    {
        public ArgumentList Arguments { get;set; }

        public string GetHelp()
        {
            return 
                $"\n-d, --data: data to encrypt, can be file or text" +
                $"\n-p1, --password1: first password of encryption, can be path or text" +
                $"\n-p2, --password2: second password of encryption, can be path or text" +
                $"\n-di, --difficulty: encryption level, is number, max recommended is 2";
        }

        public string GetName()
        {
            return "LongMethodEncryption".ToUpper();
        }

        public string GetDescription()
        {
            return "Use AES 256 bits CBC encryption to encode file or text with 2 passwords";
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
            var encryptedContent = GetByteArrayOfData();
            for (ulong currentRound = 1; currentRound <= difficultedRounds; currentRound++)
            {
                var privateKey = GeneratePrivateKey(currentRound);
                var initialVector = GenerateInitialVector(currentRound);
                encryptedContent = encryptedContent.EncodeAES256CBCRandomInfo(privateKey, initialVector, (ulong)currentRound);
                ITerminalCommand.ReportProgress(arguments, currentRound, difficultedRounds);
            }

            ITerminalCommand.ReportFinishFunction(arguments);

            return encryptedContent;
        }
    }
}
