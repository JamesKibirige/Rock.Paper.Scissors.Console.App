using System;
using System.Collections.Generic;
using System.Text;
using RockPaperScissors.ContestResults;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Players;
using RockPaperScissors.Properties;
using StructureMap;

namespace RockPaperScissors.RegistryDSL
{
    /// <summary>
    /// 
    /// </summary>
    public class ContestRegistry: Registry
    {
        /// <summary>
        /// 
        /// </summary>
        public ContestRegistry()
        {
            For<Contest>().Add<Match>().Named("Match").Ctor<int>("id").Is(1).Ctor<int>("numGames").Is(3);
            For<Contest>().Add<Game>().Named("Game");
            For<Contest>().Use("Match");
        }
    }
}
