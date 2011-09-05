using System.Text.RegularExpressions;

namespace core.roller
{
    public class RequestCommandParser
    {
        private const string PatternMatcher = @"/{2}\s*Request\s\w";

        public bool IsMatchingCommand(string message)
        { return Regex.IsMatch(message, PatternMatcher, RegexOptions.IgnoreCase); }

        public RequestCommand ParseCommandFromMessage(string message, string sender)
        {
            var rollMessage = message.Replace("//Request", "").Replace("//request", "").Trim();
            var rollCommand = new RequestCommand()
                                  {
                                      Requester = sender,
                                      RequestMessage = rollMessage
                                  };
            return rollCommand;
        }
    }
}