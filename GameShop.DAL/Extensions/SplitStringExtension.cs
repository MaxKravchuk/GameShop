using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Extensions
{
    public static class SplitStringExtension
    {
        public static string[] SplitString(this string input, string delimiter)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (delimiter == null)
            {
                throw new ArgumentNullException(nameof(delimiter));
            }

            if (delimiter.Length == 0)
            {
                throw new ArgumentException("Delimiter cannot be an empty string.");
            }

            var result = new List<string>();
            int startIndex = 0;
            int delimiterIndex;

            while ((delimiterIndex = input.IndexOf(delimiter, startIndex)) != -1)
            {
                string substring = input.Substring(startIndex, delimiterIndex - startIndex);
                result.Add(substring);
                startIndex = delimiterIndex + delimiter.Length;
            }

            if (startIndex < input.Length)
            {
                string lastSubstring = input.Substring(startIndex);
                result.Add(lastSubstring);
            }

            return result.ToArray();
        }
    }
}
