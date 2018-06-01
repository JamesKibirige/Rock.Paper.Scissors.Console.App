using System.Text.RegularExpressions;

namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// Encapsulates Validation behaviours
    /// </summary>
    public interface IValidator
    {

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
        string RequestValidInput(string validationMessage, string validationExpression);

        /// <summary>
        /// Request Valid Input from User, validates the users input using a RegEx(Regular expression) and loops until user provides valid input.
        /// </summary>
        /// <param name="validationMessage">
        /// </param>
        /// <param name="rgx">
        /// Regular expression used to parse and find matches in the users input for validation
        /// </param>
        /// <returns></returns>
        string RequestValidInput(string validationMessage, Regex rgx);
    }
}