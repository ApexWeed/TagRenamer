using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tag_Renamer
{
    public static class ArrayExtensions
    {
        public static string Implode<T>(this T[] Array, string Joiner = ", ")
        {
            var count = Array.Count();
            var current = 0;
            var sb = new StringBuilder();
            foreach (var item in Array)
            {
                sb.Append(item);
                current++;
                if (current != count)
                {
                    sb.Append(Joiner);
                }
            }

            return sb.ToString();
        }
    }
}
