namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// Encapsulate behaviours used to send and retrieve from the Console
    /// </summary>
    public interface IConsoleAdapter
    {
        /// <summary>
        /// Reads the next line of characters from the Console
        /// </summary>
        /// <returns></returns>
        string ReadLine();

        /// <summary>
        /// Writes the specified string value to the Console
        /// </summary>
        /// <param name="message"></param>
        void Write(string message);

        /// <summary>
        /// WriteLine method Writes the current line terminator, to the Console
        /// </summary>
        void WriteLine();

        /// <summary>
        /// Overloaded WriteLine method Writes the specified string value, followed by the current line terminator, to the Console
        /// </summary>
        /// <param name="message"></param>
        void WriteLine(string message);
    }
}