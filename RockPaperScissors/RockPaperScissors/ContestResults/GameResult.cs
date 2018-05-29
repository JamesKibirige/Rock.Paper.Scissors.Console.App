using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.ContestResults
{
    /// <summary>
    /// A GameResult encapsulates the result of a played game.
    /// </summary>
    public class GameResult : IContestResult
    {
        /// <summary>
        /// The id that identifies the Contest that the IContestResult instance is for
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// A game result records the outcome of a played game and can either be a win or a draw.
        /// </summary>
        public ContestOutcome Outcome { get; set; }

        /// <summary>
        /// The list of Moves that were played during the Game
        /// </summary>
        public List<IMove> Moves { get; set; }

        /// <summary>
        /// The Winning Move for the Game that was played
        /// </summary>
        public IMove WinningMove { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"></see> class.
        /// </summary>
        /// <param name="outcome"></param>
        /// <param name="gameId"></param>
        /// <param name="moves"></param>
        /// <param name="winningMove"></param>
        public GameResult(ContestOutcome outcome, int gameId, List<IMove> moves, IMove winningMove)
        {
            Outcome = outcome;
            ContestId = gameId;
            Moves = moves;
            WinningMove = winningMove;
        }

        /// <summary>
        /// Overidden ToString() method.
        /// </summary>
        /// <returns>
        /// A string that represents the current object. To return a message that represents the GameResult instance.
        /// </returns>
        public override string ToString()
        {
            var result = "";

            //Title
            result += $"Game<{ContestId}>: Result\n\n";

            //Moves
            foreach (var move in Moves)
            {
                result += move.ToString()+"\n";
            }
            result += "\n";

            //Outcome
            if (Outcome == ContestOutcome.Win)
            {
                var losingMove = Moves.First(m => m.Action != WinningMove.Action);
                result += $"{WinningMove.Action} beats {losingMove.Action}\n\n" +
                          $"Player<{WinningMove.Owner.Name}> wins Game<{ContestId}>!!!\n";
            }

            if (Outcome == ContestOutcome.Draw)
            {
                result += "Draw\n";
            }

            return result;
        }
    }
}
