using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetsGamingCenter.Api.Models
{
    public class Prize
    {
        public string PrizeID { get; set; }
        public string GameID { get; set; }
        public string PrizeName { get; set; }
        public int PrizeValue { get; set; }
        public bool IsMonetary { get; set; }
        public bool Active { get; set; }
        public string Type { get { return "prize"; } }
    }
}
