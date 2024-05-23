using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyRugby.MatchAndTrainingAmateur.MatchRecord
{
    /// <summary>
    /// Class representing a match record
    /// </summary>
    public class MatchRecord
    {
        public string TeamName { get; set; }
        public DateTime DateOfMatch { get; set; }
        public string OppositionName { get; set; }
        public string Location { get; set; }
        public string KickoffTime { get; set; }
        public string Result { get; set; }
        public string Score { get; set; }
        public int FirstHalfHomePoints { get; set; }
        public int FirstHalfAwayPoints { get; set; }
        public int SecondHalfHomePoints { get; set; }
        public int SecondHalfAwayPoints { get; set; }
        public string GameComments { get; set; }



    }
}
