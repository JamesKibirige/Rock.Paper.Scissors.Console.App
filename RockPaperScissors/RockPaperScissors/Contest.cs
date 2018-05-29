using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors
{
    /// <summary>
    /// Abstract base class encapsulating concept of a Contest which is made up of a collection of Participants and a Winner
    /// </summary>
    public abstract class Contest: IPlayable
    {
        /// <summary>
        /// Numeric identifier for a Contest
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Read only Commencement Message used to inform that a Contest is Commencing
        /// </summary>
        public string CommencementMessage
        {
            get
            {
                var instanceTypeName = this.GetType().Name;
                return $"{instanceTypeName}<{Id}>: Commence...";
            }
        }

        /// <summary>
        /// The Participants of a Contest
        /// </summary>
        public List<IPlayer> Participants { get; set; }

        /// <summary>
        /// A Contest has a winner which is decifered after a Contest has been played
        /// </summary>
        public virtual IPlayer Winner { get; set; }

        /// <summary>
        /// A match needs a User interaction controller to send and receive messages and input from the User
        /// </summary>
        public IUserInteraction Controller { get; set; }

        /// <summary>
        /// Contest outcome describing the outcome of a played Contest. A Contest outcome can be a win or draw
        /// </summary>
        public ContestOutcome Outcome { get; set; }

        /// <summary>
        /// A Contest is played according to a set of rules
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public abstract IContestResult Play(IGameRules rules);
    }
}
