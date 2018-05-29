using RockPaperScissors.Enums;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors
{
    /// <summary>
    /// A Move is modelled as a complex type that features a mapping between a Player and a GameAction.
    /// </summary>
    public class Move: IMove
    {
        /// <summary>
        /// A Move has an action representing the GameAction that is being played
        /// </summary>
        public GameAction Action { get; set; }

        /// <summary>
        /// A Move has an Owner who is the Player that made the Move
        /// </summary>
        public IPlayer Owner { get; set; }

        /// <summary>
        /// Main constructor injecting dependancies.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="owner"></param>
        public Move(GameAction action, IPlayer owner)
        {
            Action = action;
            Owner = owner;
        }

        /// <summary>
        /// Overidden implementation of ToString() that returns the Player and Action they played as a String message
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Player<{Owner.Name}>: Played <{Action.ToString()}>";
        }
    }
}
