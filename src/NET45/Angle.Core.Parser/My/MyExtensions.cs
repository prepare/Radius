using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngleSharp.Parser
{

    static class MyExtensions
    {
        public static bool Is(this String a, String b)
        {
            return a == b;
        }
        public static Boolean Isi(this String current, String other)
        {
            return String.Equals(current, other, StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsOneOf(this string current, params string[] others)
        {
            int j = others.Length;
            for (int i = 0; i < j; ++i)
            {
                if (others[i] == current)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
namespace AngleSharp.Dom.Mathml
{
    class dummy1 { }
}