using RockPaperScissors.Interfaces;
using System;
using System.Collections.Generic;

namespace RockPaperScissors
{
    /// <summary>
    /// Generic Enum Enumerator class used to enumerate Enum types
    /// </summary>
    public class EnumEnumerator<TEnum> : IEnumEnumerator<TEnum> where TEnum: Enum
    {
        /// <summary>
        /// Enumerates an enum type defined by TEnum
        /// </summary>
        /// <returns>
        /// Returns the list of values specifed in the enum type TEnum
        /// </returns>
        public List<TEnum> EnumerateEnum()
        {
            var enumValues = Enum.GetValues(typeof(TEnum));
            var result = new List<TEnum>();

            try
            {
                foreach (var value in enumValues)
                {
                    var item = (TEnum)value;
                    if ((int)value != 0)
                    {
                        result.Add(item);
                    }
                }
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }
    }
}