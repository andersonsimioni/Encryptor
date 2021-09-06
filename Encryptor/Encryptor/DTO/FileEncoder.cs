using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encryptor.Extensions;
using System.IO;

namespace Encryptor.DTO
{
    public class FileEncoder
    {
        public byte[] Content { get; set; }

        /// <summary>
        /// Create FileEncoder with content from file byte data
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileEncoder Load(string path) 
        {
            if (File.Exists(path))
            {
                return new FileEncoder()
                {
                    Content = File.ReadAllBytes(path)
                };
            }

            return null;
        }

        /// <summary>
        /// Save new encrypted file
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path) 
        {
            var newPath = $"{path}.enc";

            if (File.Exists(newPath) == false)
                File.Create(newPath).Dispose();

            File.WriteAllBytes(newPath, Content);
        }
    }
}
