using System;
using System.Collections.Generic;
using System.Text;
using RockPaperScissors.Enums;

namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// Interface that encapsulates the behaviours of ContestResult instances
    /// </summary>
    public interface IContestResult
    {
        /// <summary>
        /// The id that identifies the Contest that the IContestResult instance is for
        /// </summary>
        int ContestId { get; set; }

        /// <summary>
        /// All IContestResult instances have an outcome enumeration
        /// </summary>
        ContestOutcome Outcome { get; set; }
    }
}
