using System.Text.RegularExpressions;

namespace core.roller
{
    public class VersionCommandParser
    {
        private const string PatternMatcher = @"/{2}\s*Version";
        
        public bool IsMatchingCommand(string message)
        { return Regex.IsMatch(message, PatternMatcher, RegexOptions.IgnoreCase); }
    }
}