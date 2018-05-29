using RockPaperScissors.Enums;

namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMove
    {
        /// <summary>
        /// A Move has an action representing the GameAction that is being played
        /// </summary>
        GameAction Action { get; set; }

        /// <summary>
        /// A Move has an Owner who is the Player that made the Move
        /// </summary>
        IPlayer Owner { get; set; }

        /// <summary>
        /// returns the Player and Action they played as a String message
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}