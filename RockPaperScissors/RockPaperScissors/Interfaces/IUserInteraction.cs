using System;
using System.Runtime.Serialization.Json;

namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// The IUserInteraction interface encapsulates behaviour that deals with interacting with the User.
    /// Requests and messages can be passed between the Application and the User
    /// </summary>
    public interface IUserInteraction
    {
        /// <summary>
        /// Console Adapter used to access and execute methods on the Console in an instance fashion to avoid direct Static
        /// method calls.
        /// </summary>
        IConsoleAdapter ConsoleAdapter { get; set; }

        /// <summary>
        /// Request Input
        /// </summary>
        /// <returns></returns>
        object RequestInput(string inputMessage);

        /// <summary>
        /// Generate Output
        /// </summary>
        /// <param name="output"></param>
        void Output(object output);
    }
}