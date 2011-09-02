using System.Collections.Generic;

namespace core.roller
{
    public class DiceResults
    {
        public string Roller { get; set; }
        public IList<int> Rolls { get; set; }
        public int Bonus { get; set; }

        public DiceResults()
        {
            Roller = string.Empty;
            Rolls = new List<int>();
        }
    }
}