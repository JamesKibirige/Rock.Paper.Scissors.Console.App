using RockPaperScissors.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors
{
    /// <summary>
    /// Class that encapsulates the Rules that games are governed by
    /// </summary>
    public class GameRules: IGameRules
    {
        /// <summary>
        /// The game rules are (Key,Value) pairs that define one way mapping between two game actions. Where the game action Key beats the game action Value
        /// </summary>
        public Dictionary<GameAction, List<GameAction>> Rules { get; set; }

        /// <summary>
        /// Main constructor injecting dependancies
        /// </summary>
        /// <param name="rules"></param>
        public GameRules(Dictionary<GameAction, List<GameAction>> rules)
        {
            Rules = rules;  
        }

        /// <summary>
        /// Enumerates the items in the GameAction enumeration returning a list of the possible GameActions for the Game
        /// </summary>
        /// <returns></returns>
        public List<GameAction> PossibleGameActions()
        {
            var gameActions = Enum.GetValues(typeof(GameAction));

            var result = new List<GameAction>();
            foreach (var action in gameActions)
            {
                var anAction = (GameAction) action;
                if (anAction != GameAction.None)
                {
                    result.Add(anAction);
                }
            }

            return result;
        }
    }
}
