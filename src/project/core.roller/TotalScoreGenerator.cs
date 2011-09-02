using System.Linq;

namespace core.roller
{
    public class TotalScoreGenerator
    {
        public int GenerateTotalScore(DiceResults diceResults)
        {
            return diceResults.Bonus + diceResults.Rolls.Sum();
        }
    }
}