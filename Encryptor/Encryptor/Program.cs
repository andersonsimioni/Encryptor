using System;
using System.Collections.Generic;
using System.Linq;

namespace Encryptor
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            args = "0 -d a.txt -p1 123 -p2 321 -ocf hex".Split(' ');
#endif
            Console.Title = "Encryptor tool";
            var terminal = new Terminal.EncryptorTerminal(args);
            terminal.RunCommand();
        }
    }
}
