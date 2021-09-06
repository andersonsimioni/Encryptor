using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryptor.Functions
{
    public interface Function
    {
        public string GetName();

        public string GetHelp();

        public void Execute(string[] args);
    }
}
