using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Players;
using StructureMap;

namespace RockPaperScissors.RegistryDSL
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayerRegistry: Registry
    {
        /// <summary>
        ///
        /// </summary>
        public PlayerRegistry()
        {
            For<IPlayer>().Add<HumanPlayer>().Named("Human");
            For<IPlayer>().Add<RandomPlayer>().Named("Random").Ctor<string>("name").Is("Random").Ctor<Random>("aRandomNumberGenerator").Is(new Random());
            For<IPlayer>().Add<TacticalPlayer>().Named("Tactical").Ctor<string>("name").Is("Tactical").Ctor<Stack<IMove>>("moves").Is(new Stack<IMove>())
                .Ctor<Random>("aRandomNumberGenerator").Is(new Random());
            For<IPlayer>().Use("Human");
            For<IMove>().Use<Move>();
        }
    }
}