using System;
using System.Collections.Generic;
using System.Text;

namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// Generic Enum Enumerator that encapsulates behaviours useful for Enumerating Enums
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public interface IEnumEnumerator<TEnum>
    {
        /// <summary>
        /// Method used to enumerate an Enum type based on the type parameter TEnum.
        /// </summary>
        /// <returns>
        /// Returns an ordered List of TEnum objects.
        /// </returns>
        List<TEnum> EnumerateEnum();
    }
}
