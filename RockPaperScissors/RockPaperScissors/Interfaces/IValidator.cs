namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validates an input string using a regular expression
        /// </summary>
        /// <returns></returns>
        bool ValidateInput(string input, string regex);
    }
}