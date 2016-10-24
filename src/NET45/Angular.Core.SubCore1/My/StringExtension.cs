using System;
namespace AngleSharp
{
   
    static class StringExtensions
    {
        [MyAggressiveMethodInlining]
        public static Boolean Is(this String current, String other)
        {
            return String.Equals(current, other, StringComparison.Ordinal);
        }
    }
}