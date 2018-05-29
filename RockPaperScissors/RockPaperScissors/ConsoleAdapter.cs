using System;
using System.Collections.Generic;
using System.Text;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors
{
    /// <summary>
    /// Concrete implementation of IConsoleAdapter that wraps the stadard output Console class and its static methods in an Adapter.
    /// ConsoleAdapter is an Adapter for the static class System.Console and its static methods.
    /// </summary>
    public class ConsoleAdapter : IConsoleAdapter
    {
        /// <summary>
        /// Reads the next line of characters from the Console
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Writes the specified string value to the Console
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            Console.Write(message);
        }

        /// <summary>
        /// WriteLine method Writes the current line terminator, to the Console
        /// </summary>
        public void WriteLine()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the Console
        /// </summary>
        /// <param name="message"></param>
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
