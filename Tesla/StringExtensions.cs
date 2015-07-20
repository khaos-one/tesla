using System.Text;

namespace Tesla
{
    public static class StringExtensions
    {
        public static byte[] ToBytes(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        public static byte[] ToBytes(this string str)
        {
            return ToBytes(str, Encoding.UTF8);
        }

        public static string[] SplitWildcard(this string str, string pattern)
        {
            if (pattern.Length > str.Length)
                return null;

            unchecked
            {
                var accumulator = new char[str.Length];
                var accumulatingWildcard = false;
                int i = 0, j = 0, k = 0, m = 0;

                for (; i < pattern.Length; i++)
                {
                    if (pattern[i] == '*')
                        m++;
                }

                var matchGroups = new string[m];

                for (i = 0, m = 0; i < str.Length;)
                {
                    if (!accumulatingWildcard && pattern[j] == '*')
                    {
                        accumulatingWildcard = true;
                        j++;
                    }

                    if (accumulatingWildcard)
                    {
                        if (j >= pattern.Length || pattern[j] != str[i])
                        {
                            accumulator[k] = str[i];
                            i++;
                            k++;

                            if (i == str.Length)
                                matchGroups[m] = new string(accumulator, 0, k);
                        }
                        else
                        {
                            matchGroups[m] = new string(accumulator, 0, k);
                            m++;
                            k = 0;
                            accumulatingWildcard = false;
                        }

                        continue;
                    }

                    if (pattern[j] != str[i])
                    {
                        return null;
                    }

                    i++;
                    j++;
                }

                return matchGroups;
            }
        }
    }
}
