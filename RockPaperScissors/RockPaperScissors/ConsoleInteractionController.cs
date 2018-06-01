using System;
using System.Text.RegularExpressions;
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
        /// Request Valid Input from User, validates the users input using a custom validation expression and loops until user provides valid input.
        /// </summary>
        /// <param name="validationMessage">
        /// This is a string message used to request that the user enters Valid input and would typically specify the rules for valid input.
        /// </param>
        /// <param name="validationExpression">
        /// This is a custom string expression that is used to constrain the input from the user. The user must provide input that is a substring of the validation expression.
        /// </param>
        /// <returns>
        /// Returns a validated string from the User
        /// </returns>
        public string RequestValidInput(string validationMessage, string validationExpression)
        {
            var userInput = "";
            var isValid = false;
            validationExpression = validationExpression.ToLower();

            do
            {
                ConsoleAdapter.WriteLine(validationMessage);
                userInput = ConsoleAdapter.ReadLine();
                isValid = validationExpression.Contains(userInput);
            } while (!isValid);

            return userInput;
        }

        /// <summary>
        /// Request Valid Input from User, validates the users input using a RegEx(Regular expression) and loops until user provides valid input.
        /// </summary>
        /// <param name="validationMessage">
        /// </param>
        /// <param name="rgx">
        /// Regular expression used to parse and find matches in the users input for validation
        /// </param>
        /// <returns></returns>
        public string RequestValidInput(string validationMessage, Regex rgx)
        {
            var userInput = "";
            var isValid = false;

            do
            {
                ConsoleAdapter.WriteLine(validationMessage);
                userInput = ConsoleAdapter.ReadLine();
                isValid = rgx.IsMatch(userInput);
            } while (!isValid);

            return userInput;
        }
    }
}
