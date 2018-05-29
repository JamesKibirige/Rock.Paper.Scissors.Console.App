using System.Collections.Generic;
using RockPaperScissors.Enums;

namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// Encapsulates the behaviours for Game Rule instances
    /// </summary>
    public interface IGameRules
    {
        /// <summary>
        /// The game rules are (Key,Value) pairs that define a one way mapping between a game action(Key) and a collection of game actions(Values) that the action(Key) beats.
        /// </summary>
        Dictionary<GameAction, List<GameAction>> Rules { get; set; }

        /// <summary>
        /// Enumerates the GameAction enumeration returning a list of the possible GameActions for the Game
        /// </summary>
        /// <returns></returns>
        List<GameAction> PossibleGameActions();
    }
}