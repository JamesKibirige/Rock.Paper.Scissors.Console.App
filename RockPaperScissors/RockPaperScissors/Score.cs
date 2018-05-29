using System.Collections.Generic;
using System.Linq;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors
{
    /// <summary>
    /// Score to keep track of player acheivement in Matches
    /// </summary>
    public class Score: IScore
    {
        /// <summary>
        /// A Score features a Dictionary of Player scores that are described as (Key,Value) pairs (IPlayer,int) where Key is Player and Value is the num of Games won
        /// </summary>
        public Dictionary<IPlayer,int> Scores { get; set; }

        /// <summary>
        /// Main constructor for Score injecting the instance dependancies
        /// </summary>
        /// <param name="scores"></param>
        public Score(Dictionary<IPlayer, int> scores)
        {
            Scores = scores;
        }

        /// <summary>
        /// Overridden ToString() method returns representation of Score instance as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = "";

            result = Scores.OrderByDescending(s => s.Value).Aggregate(result, (current, score) => current + $"Player<{score.Key.Name}>: Score <{score.Value}>\n");

            return result;
        }
    }
}
