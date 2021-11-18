using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryptor.Terminal.Commands
{
    public class ArgumentList
    {
        private readonly Dictionary<string,string> Arguments;

        /// <summary>
        /// If not contains return empty string
        /// </summary>
        /// <param name="argsNames"></param>
        /// <returns></returns>
        public string GetArgumentValue(params string[] argsNames) 
        {
            foreach (var item in argsNames)
                if (this.Arguments.ContainsKey(item))
                    return this.Arguments[item];

            foreach (var item in argsNames)
                if (this.Arguments.ContainsKey(item.ToUpper()))
                    return this.Arguments[item.ToUpper()];

            return null;
        }

        public string GetCommandName() 
        {
            return GetArgumentValue("CommandName");
        }

        /// <summary>
        /// Create pair key values list
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private Dictionary<string, string> CreateKeyValuePairs(params string[] args)
        {
            try
            {
                var values = new Dictionary<string, string>();
                var concat = string.Concat(args);

                var commandName = args[0];
                values.Add("CommandName".ToUpper(), commandName);

                for (int i = 1; i < args.Length; i += 2)
                    values.Add(args[i].Replace("-", "").ToUpper(), args[i+1]);

                return values;
            }
            catch (Exception ex)
            {
                throw new Exception($"Terminal error on convert arguments to dictonary values, {ex.Message}");
            }
        }

        public ArgumentList(params string[] args) 
        {
            this.Arguments = CreateKeyValuePairs(args);
        }
    }
}
