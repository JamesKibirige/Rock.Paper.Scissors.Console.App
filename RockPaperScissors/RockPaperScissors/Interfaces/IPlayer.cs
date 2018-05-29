using RockPaperScissors.Interfaces;

namespace RockPaperScissors
{
    /// <summary>
    /// Interface that encapsulates and describes the behaviours of an IPlayer
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// A Player has a Name represented by a String literal
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Players use Game Rules to generate moves
        /// </summary>
        IGameRules Rules { get; set; }

        /// <summary>
        /// A Player can generate a Move
        /// </summary>
        /// <returns></returns>
        Move SelectMove();
    }
}
