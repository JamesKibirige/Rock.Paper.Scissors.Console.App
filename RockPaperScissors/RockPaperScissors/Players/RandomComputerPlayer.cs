using System;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.Players
{
    /// <summary>
    /// A Random Computer Player is a Player that selects Moves at Random.
    /// </summary>
    public class RandomComputerPlayer : Player
    {
        /// <summary>
        /// Random number generator used by RandomComputerPlayer instances to select moves by random
        /// </summary>
        public Random RandomNumberGenerator { get; set; }

        /// <summary>
        /// Main constructor injecting in dependancies
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rules"></param>
        /// <param name="aRandomNumberGenerator"></param>
        public RandomComputerPlayer(string name, IGameRules rules, Random aRandomNumberGenerator)
        {
            Name = name;
            Rules = rules;
            RandomNumberGenerator = aRandomNumberGenerator;
        }

        /// <summary>
        /// Used to select moves, Random computer players select moves at random based on the list of possible Game Actions defined by
        /// injected GameRules instance
        /// </summary>
        /// <returns></returns>
        public override Move SelectMove()
        {
            //Get List of Possible Game Actions from GameRules rules instance
            var possibleActions = Rules.PossibleGameActions();

            //Generate a random number between 1 and total number of game actions using RandomNumberGenerator
            var random = RandomNumberGenerator.Next(1,possibleActions.Count + 1);

            //Cast random number to GameAction enum instance
            var gameAction = (GameAction) random;

            //set up Move based on randomly selected GameAction and this Player instance
            var playerMove = new Move(gameAction,this);

            //return Move
            return playerMove;
        }
    }
}
