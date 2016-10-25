using System;
namespace AngleSharp.Extensions
{
    static class CssStringExtensions
    {
        /// <summary>
        /// Creates a CSS function from the string with the given argument.
        /// </summary>
        /// <param name="value">The CSS function name.</param>
        /// <param name="argument">The CSS function argument.</param>
        /// <returns>The CSS function string.</returns>
        public static String CssFunction(this String value, String argument)
        {
            return String.Concat(value, "(", argument, ")");
        }
    }
}