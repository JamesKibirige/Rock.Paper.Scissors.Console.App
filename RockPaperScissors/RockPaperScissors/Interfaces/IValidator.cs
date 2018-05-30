namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// Encapsulates Validation behaviours
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Request Valid Input from User, validates user input then loops if invalid, if valid returns a valid string. The Validation Expression constrains the user input
        /// </summary>
        /// <returns></returns>
        string RequestValidInput(string validationMessage, string validationExpression);
    }
}