using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors.ContestResults
{
    /// <summary>
    /// Encapsulation of the information in relation the result of a Match
    /// </summary>
    public class MatchResult: IContestResult
    {
        /// <summary>
        /// The id that identifies the Contest that the IContestResult instance is for
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// Winner of the match may be NUll is the Match Outcome is a draw
        /// </summary>
        public IPlayer Winner { get; set; }

        /// <summary>
        /// What was the Match Score
        /// </summary>
        public IScore Score { get; set; }

        /// <summary>
        /// Player Participants in the Match
        /// </summary>
        public List<IPlayer> Participants { get; set; }

        /// <summary>
        /// The Outcome of the Match
        /// </summary>
        public ContestOutcome Outcome { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"></see> class.
        /// </summary>
        /// <param name="contestId"></param>
        /// <param name="winner"></param>
        /// <param name="score"></param>
        /// <param name="participants"></param>
        /// <param name="outcome"></param>
        public MatchResult(int contestId, IPlayer winner, IScore score, List<IPlayer> participants, ContestOutcome outcome)
        {
            ContestId = contestId;
            Winner = winner;
            Score = score;
            Participants = participants;
            Outcome = outcome;
        }

        /// <summary>
        ///  Overidden ToString() method.
        /// </summary>
        /// <returns>
        /// A string that represents the current object. To return a message that represents the MatchResult instance.
        /// </returns>
        public override string ToString()
        {
            var result = "";

            //Title
            result += $"Match<{ContestId}>: Result\n\n";

            //Outcome
            result += $"Match Outcome<{Outcome}>\n\n";

            //Score
            result += Score.ToString();
            result += "\n";

            //Winner
            if (Outcome == ContestOutcome.Win)
            {
                result+= $"Player<{Winner.Name}> Is the Winner!!!\n";
            }

            return result;
        }
    }
}
