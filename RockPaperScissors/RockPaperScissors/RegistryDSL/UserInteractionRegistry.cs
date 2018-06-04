using System;
using RockPaperScissors.Interfaces;
using StructureMap;
using StructureMap.Pipeline;

namespace RockPaperScissors.RegistryDSL
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInteractionRegistry : Registry
    {
        /// <summary>
        /// 
        /// </summary>
        public UserInteractionRegistry()
        {
            For<IUserInteraction>().Use<ConsoleInteractionController>();
            For<IConsoleAdapter>().Use<ConsoleAdapter>();
        }
    }
}