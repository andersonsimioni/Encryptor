using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using System.IO;
using System.Diagnostics;

namespace Encryptor.Terminal
{
    public class EncryptorTerminal
    {
        private enum OutputConsoleFormat {
            DefaultStringConvert,
            HexString
        }
        private static (string name, OutputConsoleFormat outputFormat)[] formatsNames = new (string name, OutputConsoleFormat outputFormat)[]
        {
            ("hex", OutputConsoleFormat.HexString),
            ("text", OutputConsoleFormat.DefaultStringConvert)
        };

        private Commands.ITerminalCommand[] Commands;
        private Commands.ArgumentList Arguments;

        /// <summary>
        /// Load all commands of program
        /// </summary>
        private void LoadCommands() 
        {
            this.Commands = new Commands.ITerminalCommand[] 
            {
                new Commands.LongMethodEncryption(),
                new Commands.LongMethodDecryption()
            };
        }

        /// <summary>
        /// if not exist, return empty string
        /// </summary>
        /// <returns></returns>
        private string GetOutputPath()
        {
            var value = this.Arguments.GetArgumentValue("o", "output");
            return value;
        }

        /// <summary>
        /// Get help guide
        /// </summary>
        /// <returns></returns>
        private string Help() {
            var helpContent = "";
            for (int i = 0; i < this.Commands.Length; i++)
            {
                var c = this.Commands[i];
                helpContent += ($"\n### {i} | {c.GetName()} ###\n");
                helpContent += ($"-> {c.GetDescription()}\n");
                helpContent += ($"{c.GetHelp()}\n");
            }
            helpContent += ($"\n\n-o, --output: is a path file to write results bytes encoded content, if not set will return hex string in terminal window");
            helpContent += ($"\n\n-ocf, --outputConsoleFormat: use to set output format of terminal, values: { string.Concat(formatsNames.Select(f=>f.name+"; ")) }");

            return helpContent;
        }

        private OutputConsoleFormat GetConsoleOutputFormat() 
        {
            var format = this.Arguments.GetArgumentValue("outputConsoleFormat","ocf") ?? "";
            if (string.IsNullOrEmpty(format))
                return OutputConsoleFormat.DefaultStringConvert;

            if (formatsNames.Any(f => f.name == format))
                return formatsNames.FirstOrDefault(f => f.name == format).outputFormat;
            else
                return OutputConsoleFormat.DefaultStringConvert;
        }

        /// <summary>
        /// Run console command
        /// </summary>
        public void RunCommand() 
        {
            var name = Arguments.GetCommandName().ToUpper();
            var commandID = 0;
            if (int.TryParse(name, out commandID))
                name = this.Commands[commandID].GetName();

            if (name == "help".ToUpper())
            {
                Console.WriteLine(Help());
                return;
            }
            
            var command = this.Commands.FirstOrDefault(c => c.GetName() == name);
            if (command == null)
                throw new Exception("Command not found");

            var format = GetConsoleOutputFormat();
            var outputFile = GetOutputPath();

            var stp = new Stopwatch();
            stp.Start();
            var result = command.Execute(this.Arguments);
            stp.Stop();

            if (string.IsNullOrEmpty(outputFile) == false)
                File.WriteAllBytes(outputFile, result);

            var ret = "";
            switch (format)
            {
                case OutputConsoleFormat.DefaultStringConvert:
                    ret= result.ConvertToString();
                    break;
                case OutputConsoleFormat.HexString:
                    ret = result.ConvertToHexString();
                    break;
                default:
                    ret = result.ConvertToString();
                    break;
            }

            Console.WriteLine($"\n\n{ret}");
            Console.WriteLine("\n\n\n### END PROGRAM ###");
            Console.WriteLine($"\ntime...: {stp.ElapsedMilliseconds / 1000}s");
        }

        public EncryptorTerminal(params string[] args) 
        {
            try
            {
                if ((args?.Length ?? 0) == 0)
                    throw new Exception("Invalid argument list");

                this.Arguments = new Commands.ArgumentList(args);
                LoadCommands();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Some error ocurred, {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
