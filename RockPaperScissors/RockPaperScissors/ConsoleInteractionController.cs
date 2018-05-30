using System;
using RockPaperScissors.Interfaces;

namespace RockPaperScissors
{
    /// <summary>
    /// The ConsoleInteractionController class encapsulates User interactions via the System Console so that user interaction code can be centralised in an Object Orientated way
    /// </summary>
    public class ConsoleInteractionController : IUserInteraction
    {
        /// <summary>
        /// Console Adapter used to access and execute methods on the Console in an instance fashion to avoid direct Static
        /// method calls.
        /// </summary>
        public IConsoleAdapter ConsoleAdapter { get; set; }

        /// <summary>
        /// Main constructor injecting instance dependancies
        /// </summary>
        /// <param name="consoleAdapter"></param>
        public ConsoleInteractionController(IConsoleAdapter consoleAdapter)
        {
            ConsoleAdapter = consoleAdapter;
        }

        /// <summary>
        /// Generate Output
        /// </summary>
        /// <param name="output"></param>
        public void Output(object output)
        {
            try
            {
                ConsoleAdapter.WriteLine(output.ToString());
                ConsoleAdapter.WriteLine();
                ConsoleAdapter.WriteLine("Press Any Key To continue...");
                ConsoleAdapter.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Request Input from the user with a message.
        /// </summary>
        /// <param name="inputMessage"></param>
        /// <returns></returns>
        public object RequestInput(string inputMessage)
        {
            try
            {
                ConsoleAdapter.WriteLine(inputMessage);
                return ConsoleAdapter.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationMessage"></param>
        /// <param name="validationExpression"></param>
        /// <returns></returns>
        public string RequestValidInput(string validationMessage, string validationExpression)
        {
            //do
            //Request Valid Input from User
            //Validate Users Input
            //while input invalid

            //return validated string
            throw new NotImplementedException();
        }
    }
}
