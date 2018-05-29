namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validates and returns an input string based on regex, otherwise requests
        /// </summary>
        /// <returns></returns>
        string ValidateInput(string input, string regEx);
    }
}