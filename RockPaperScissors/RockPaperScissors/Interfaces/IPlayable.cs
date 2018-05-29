using System;
using System.Collections.Generic;
using System.Text;

namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// Interface used to describe the behaviour of Playable instances
    /// </summary>
    public interface IPlayable
    {
        /// <summary>
        /// Playable objects can be played accoriding to a set of Game rules
        /// </summary>
        /// <returns></returns>
        IContestResult Play(IGameRules rules);
    }
}
