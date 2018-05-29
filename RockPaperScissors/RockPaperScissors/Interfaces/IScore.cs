using System.Collections.Generic;

namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// Interface that encapsulates behaviours for Score instances
    /// </summary>
    public interface IScore
    {
        /// <summary>
        /// A Score features a Dictionary of Player scores that are described as (Key,Value) pairs (IPlayer,int) where Key = Player and Value = number games won
        /// </summary>
        Dictionary<IPlayer, int> Scores { get; set; }

        /// <summary>
        /// Used to return a Score instance as a string
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}