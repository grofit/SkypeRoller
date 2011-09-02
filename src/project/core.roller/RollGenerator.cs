using System;

namespace core.roller
{
    public class RollGenerator
    {
        private Random randomizer = new Random();

        public DiceResults GenerateRollResults(DiceCommand command)
        {
            var diceResults = new DiceResults()
            {                                      
                Roller = command.Roller,
                Bonus = command.Bonus
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
