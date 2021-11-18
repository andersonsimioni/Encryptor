using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Encryptor.Terminal.Commands
{
    public interface ITerminalCommand
    {
        public ArgumentList Arguments { get; set; }

        public byte[] GetOutput() 
        {
            return null;
        }

        public string GetName();

        public string GetDescription();

        public string GetHelp();

        public byte[] Execute(ArgumentList arguments);

        /// <summary>
        /// Report current progress of functions process
        /// </summary>
        /// <param name="current"></param>
        /// <param name="max"></param>
        public static void ReportProgress(ArgumentList arguments, float current, float max)
        {
            var progress = 0f;
            if (max != 0)
                progress = 100f * current / max;

            progress = (float)Math.Round(progress, 2);

            Console.WriteLine($"[PROGRESS]: {progress}%");
        }

        /// <summary>
        /// Call this void when function processing is finished
        /// </summary>
        public static void ReportFinishFunction(ArgumentList arguments)
        {
            if (string.IsNullOrEmpty(arguments.GetArgumentValue("o", "output")))
                return;

            Console.Clear();
            Console.WriteLine("### Process Finished ###");
        }
    }
}
