using System.Collections.Generic;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Properties;
using StructureMap;

namespace RockPaperScissors.RegistryDSL
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationRegistry: Registry
    {
        /// <summary>
        /// 
        /// </summary>
        public ApplicationRegistry()
        {
            For<IRunnable>().Use<Application>().Named("Application")
                .Ctor<string>("title").Is(Resources.ApplicationTitle)
                .Ctor<string>("author").Is(Resources.ApplicationAuthor)
                .Ctor<List<IPlayer>>("players").Is(new List<IPlayer>());

            var rulesDictionary = new Dictionary<GameAction, List<GameAction>>()
            {
                {
                    GameAction.Rock,
                    new List<GameAction>() {GameAction.Scissors}
                },
                {
                    GameAction.Paper,
                    new List<GameAction>() {GameAction.Rock}
                },
                {
                    GameAction.Scissors,
                    new List<GameAction>() {GameAction.Paper}
                }
            };
            For<IGameRules>().Use<GameRules>().Named("GameRules").Ctor<Dictionary<GameAction, List<GameAction>>>("rules").Is(rulesDictionary);
            For<IScore>().Use<Score>().Named("Score").Ctor<Dictionary<IPlayer, int>>("scores").Is(new Dictionary<IPlayer, int>());
        }
    }
}