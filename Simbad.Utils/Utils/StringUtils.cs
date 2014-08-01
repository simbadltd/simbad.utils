using System;

namespace Simbad.Utils.Utils
{
    public static class StringUtils
    {
        public static Boolean MatchWildcard(String pattern, String input)
        {
            if (String.CompareOrdinal(pattern, input) == 0)
            {
                return true;
            }
            
            if (String.IsNullOrEmpty(input))
            {
                return String.IsNullOrEmpty(pattern.Trim(new[] { '*' }));
            }
            
            if (pattern.Length == 0)
            {
                return false;
            }
            
            if (pattern[0] == '?')
            {
                return MatchWildcard(pattern.Substring(1), input.Substring(1));
            }
            
            if (pattern[pattern.Length - 1] == '?')
            {
                return MatchWildcard(pattern.Substring(0, pattern.Length - 1), input.Substring(0, input.Length - 1));
            }
            
            if (pattern[0] == '*')
            {
                return MatchWildcard(pattern.Substring(1), input) || MatchWildcard(pattern, input.Substring(1));
            }
            
            if (pattern[pattern.Length - 1] == '*')
            {
                return MatchWildcard(pattern.Substring(0, pattern.Length - 1), input) ||
                       MatchWildcard(pattern, input.Substring(0, input.Length - 1));
            }
            
            return pattern[0] == input[0] && MatchWildcard(pattern.Substring(1), input.Substring(1));
        }
    }
}