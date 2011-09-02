namespace core.roller
{
    public class MessageParser
    {
        public bool IsValidRollCommand(string message)
        { return (message.Substring(0, 2) == "//") && (message.Contains("d")); }

        private bool HasBonusInCommand(string formattedCommand)
        { return (formattedCommand.Contains("+") || formattedCommand.Contains("-")); }

        private string FormatMessageToCommand(string message)
        { return message.Remove(0, 2).Replace(" ", "").ToLower(); }

        public DiceCommand ParseCommandFromMessage(string message, string sender)
        {
            var formattedCommand = FormatMessageToCommand(message);
            var diceCommand = new DiceCommand();

            diceCommand.Roller = sender;
            diceCommand.Dice = ParseDiceCount(formattedCommand);
            diceCommand.Sides = ParseDiceSides(formattedCommand);
            
            if(HasBonusInCommand(formattedCommand))
            { diceCommand.Bonus = ParseBonus(formattedCommand); }

            return diceCommand;
        }

        private int ParseBonus(string formattedCommand)
        {
            if(formattedCommand.Contains("+"))
            { return int.Parse(formattedCommand.Split('+')[1]); }
            return int.Parse("-" + formattedCommand.Split('-')[1]);
        }

        private int ParseDiceSides(string formattedCommand)
        {
            var splitData = formattedCommand.Split('d');
            if(HasBonusInCommand(formattedCommand))
            {
                if(formattedCommand.Contains("+"))
                { return int.Parse(splitData[1].Split('+')[0]); }
                return int.Parse(splitData[1].Split('-')[0]);
            }
            return int.Parse(splitData[1]);
        }

        private int ParseDiceCount(string formattedCommand)
        {
            var splitData = formattedCommand.Split('d');
            return int.Parse(splitData[0]);
        }
    }
}