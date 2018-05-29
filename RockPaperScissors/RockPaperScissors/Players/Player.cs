using RockPaperScissors.Interfaces;

namespace RockPaperScissors.Players
{
    /// <summary>
    /// Abstract base class representing a generic Player in the Player hierarchy. Implements the IPlayer Interface. A Player can select moves
    /// </summary>
    public abstract class Player : IPlayer
    {
        /// <summary>
        /// A Player has a Name represented by a String literal
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Players use Game Rules to generate moves
        /// </summary>
        public virtual IGameRules Rules { get; set; }

        /// <summary>
        /// A Player can select a Move to play in a game.
        /// </summary>
        /// <returns></returns>
        public abstract Move SelectMove();
    }
}
