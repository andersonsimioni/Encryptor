using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Encryptor.Functions
{
    public abstract class Function
    {
        private Stopwatch TimeAnalizer;

        /// <summary>
        /// Return function name to call
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        /// <summary>
        /// Return function help guide
        /// </summary>
        /// <returns></returns>
        public abstract string GetHelp();

        /// <summary>
        /// Call when function start
        /// </summary>
        public void ReportInitFunction()
        {
            TimeAnalizer = new Stopwatch();
            TimeAnalizer.Start();
        }

        /// <summary>
        /// Report current progress of functions process
        /// </summary>
        /// <param name="current"></param>
        /// <param name="max"></param>
        public void ReportProgress(float current, float max)
        {
            var progress = 0f;
            if (max != 0)
                progress = 100f * current / max;

            progress = (float)Math.Round(progress, 2);

            //Console.Clear();
            Console.WriteLine($"[{GetName()}]: {progress}%");
        }

        /// <summary>
        /// Call this void when function processing is finished
        /// </summary>
        public void ReportFinishFunction() 
        {
            TimeAnalizer.Stop();

            Console.Clear();
            Console.WriteLine("### Process Finished ###");
            Console.WriteLine($"Total time: {TimeAnalizer.ElapsedMilliseconds/1000}s");
        }

        /// <summary>
        /// Execute function method
        /// </summary>
        /// <param name="args"></param>
        public abstract void Execute(string[] args);
    }
}
