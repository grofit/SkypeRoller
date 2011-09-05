using System;

namespace core.roller
{
    public class RollGenerator
    {
        private Random randomizer = new Random();

        public string GenerateRollString(DiceCommand command)
        {
            return string.Format("{0}d{1}{2}{3}", command.Dice, command.Sides, 
                                                  (command.Bonus > 0) ? "+" : "",
                                                  (command.Bonus != 0) ? command.Bonus.ToString() : "");
        }

        public DiceResults GenerateRollResults(DiceCommand command)
        {
            var diceResults = new DiceResults()
            {                                      
                Roller = command.Roller,
                Bonus = command.Bonus,
                OriginalRoll = GenerateRollString(command)
            };

            for(var i=0;i<command.Dice;i++)
            {
                var outputRoll = randomizer.Next(1, command.Sides);
                diceResults.Rolls.Add(outputRoll);
            }

            return diceResults;
        }
    }
}
