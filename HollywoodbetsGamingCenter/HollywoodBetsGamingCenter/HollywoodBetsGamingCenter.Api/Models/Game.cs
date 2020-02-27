using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetsGamingCenter.Api.Models
{
    public class Game
    {
        public string GameID { get; set; }
        public string GameName { get; set; }
        public int DailyBudget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsResulted { get; set; }
        public string Type { get { return "game"; } }
    }
}
