using System;
using System.Text;

namespace core.roller
{
    public class RollOutputGenerator
    {
        private TotalScoreGenerator totalScoreGenerator;

        public RollOutputGenerator(TotalScoreGenerator totalScoreGenerator)
        {
            this.totalScoreGenerator = totalScoreGenerator;
        }

        public string GenerateOutputMessage(DiceResults diceResult)
        {
            var totalScore = totalScoreGenerator.GenerateTotalScore(diceResult);
            var outputMessage = new StringBuilder();

            outputMessage.AppendFormat("{0} rolled {1}{2}", diceResult.Roller, totalScore, Environment.NewLine);
            outputMessage.AppendFormat("- Consisting of ");
            foreach(var roll in diceResult.Rolls) { outputMessage.AppendFormat("{0} ", roll); }

            if(diceResult.Bonus != 0)
            { outputMessage.AppendFormat("with a bonus of {0}", diceResult.Bonus); }

            return outputMessage.ToString();
        }
    }
}