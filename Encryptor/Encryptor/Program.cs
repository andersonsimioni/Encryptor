using System;
using System.Collections.Generic;
using Encryptor.Extensions;
using System.Linq;

namespace Encryptor
{
    class Program
    {
        static Functions.Function[] Functions =
        {
            new Functions.HardEnconding()
        };

        static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(args[0]))
                throw new Exception("Invalid function name/id");

            if(args[0] == "help")
            {
                for(var i = 0; i < Functions.Length; i++)
                    Console.WriteLine($"[{i} - {Functions[i].GetName()}]: {Functions[i].GetHelp()}");
                return;
            }

            var functionName = args[0];
            var function = Functions.FirstOrDefault(f => f.GetName() == functionName);

            var functionId = -1;
            if (function == null && int.TryParse(args[0], out functionId) == false)
                throw new Exception("Function not found");

            function = Functions[functionId];
            function.Execute(args);
        }
    }
}
