using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryptor.Functions
{
    public class HardEnconding : Function
    {
        public string GetHelp()
        {
            return "HardEnconding \"file path\" \"first password\" \"second password\" \"numeric password\"";
        }

        public string GetName()
        {
            return "HardEnconding";
        }


        public void Execute(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
