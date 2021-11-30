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
            string command = "";
            Console.Write("command...?");
            command = Console.ReadLine();
            args = command.Split(' ');
#endif
            Console.Title = "Encryptor tool";
            var terminal = new Terminal.EncryptorTerminal(args);
            terminal.RunCommand();
        }
    }
}
