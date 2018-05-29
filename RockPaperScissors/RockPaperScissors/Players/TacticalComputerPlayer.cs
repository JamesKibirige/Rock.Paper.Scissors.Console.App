using System;
using System.Collections.Generic;
using System.Linq;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.Players
{
    /// <summary>
    /// A Tactical Computer Player is a RandomComputerPlayer that selects moves tactically
    /// </summary>
    public class TacticalComputerPlayer : RandomComputerPlayer
    {
        /// <summary>
        /// A Tactical Computer Player remembers the history of moves that they have played during a match.
        /// LIFO collection of Moves representing the Moves a player has played.
        /// </summary>
        public Stack<IMove> Moves { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rules"></param>
        /// <param name="moves"></param>
        /// <param name="aRandomNumberGenerator"></param>
        public TacticalComputerPlayer(string name, IGameRules rules, Stack<IMove> moves, Random aRandomNumberGenerator)
        :base(name, rules, aRandomNumberGenerator)
        {
            Moves = moves;
        }

        /// <summary>
        /// A Tactical Computer Player selects moves tactically based on the Moves they have played and the game rules
        /// </summary>
        /// <returns></returns>
        public override Move SelectMove()
        {
            //Selects moves tactically
            //The tactical computer player should always select the choice that would have beaten its last choice,
            //e.g. if it played Scissors in game 2, it should play Rock in game 3. It does not need to take the other 
            //Select Move that beats last Move
            Move playerMove;

            if (Moves.Count == 0)
            {
                //  Select initial Move Randomly - Inital move is when this.Moves.Count = 0
                var possibleActions = Rules.PossibleGameActions();
                var random = RandomNumberGenerator.Next(1, possibleActions.Count + 1);
                var gameAction = (GameAction)random;
                playerMove = new Move(gameAction, this);

                //Push Move onto Moves Stack so that Player has memory of the Moves they select in subsequent calls
                Moves.Push(playerMove);

                //return Move
                return playerMove;
            }
            else
            {
                //  Subsequent Moves are tactical
                var lastMove = Moves.Pop();

                //Get the GameAction that beats the action used in the last move
                var action = Rules.Rules.Where(r => r.Value.First() == lastMove.Action).Select(s => s.Key).FirstOrDefault();

                //Set up the player Move with tactically selected action
                playerMove = new Move(action, this);

                //Push the Move onto the Moves stack so the Player remembers this last move
                Moves.Push(playerMove);

                return playerMove;
            }
        }
    }
}
