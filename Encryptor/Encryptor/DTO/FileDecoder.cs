using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Encryptor.DTO
{
    public class FileDecoder
    {
        public byte[] Content { get; set; }

        /// <summary>
        /// Create FileEncoder with content from encoded file byte data
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileDecoder Load(string path) 
        {
            if (File.Exists(path)) 
            {
                return new FileDecoder()
                {
                    Content = File.ReadAllBytes(path)
                };
            }

            return null;
        }

        /// <summary>
        /// Save decoded byte data into old original file
        /// </summary>
        /// <param name="encodedPath"></param>
        public void Save(string encodedPath) 
        {
            var newPath = encodedPath;
            if (newPath.EndsWith(".enc"))
                newPath = newPath.Substring(0, newPath.Length - 3);

            if(File.Exists(newPath) == false)
                File.Create(newPath).Dispose();

            File.WriteAllBytes(newPath, Content);
        }
    }
}
