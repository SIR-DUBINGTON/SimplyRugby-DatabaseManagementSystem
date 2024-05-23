using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyRugby.MatchAndTrainingAmateur.GameOption
{
    /// <summary>
    /// Class to represent a game option
    /// </summary>
    public class GameOption
    {
        public int GameID { get; set; }

        public DateTime GameDate { get; set; }
        public string GameDisplay { get; set; }

        public string TeamName { get; set; }
    }
}
