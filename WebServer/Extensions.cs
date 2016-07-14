using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public static class Extensions
    {
        /// <summary> 
        /// Return everything to the left of the first occurrence of the specified string,
        /// or the entire source string.
        /// </summary> 
        /// <param name="src">source string </param>
        /// <param name="s">reference string</param>
        /// <returns>
        ///     New string left of the occurrence.
        /// </returns>
        public static string LeftOf(this string src, string s)
        {
            string ret = src;
            int idx = src.IndexOf(s);
            if (idx != -1)
            {
                ret = src.Substring(0, idx);
            }

            return ret;
        }

        /// <summary>
        /// Return everything to the right of the first occurrence of the specified string,
        /// or an empty string.
        /// </summary>
        /// <param name="src">source string </param>
        /// <param name="s">reference string</param>
        /// <returns>
        ///     New string right of the occurrence.
        /// </returns>
        public static string RightOf(this string src, string s)
        {
            string ret = string.Empty;
            int idx = src.IndexOf(s);
            if (idx != -1)
            {
                ret = src.Substring(idx + s.Length);
            }

            return ret;
        }
    }
}
