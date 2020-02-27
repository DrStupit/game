using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetsGamingCenter.Api.Models
{
    public class User
    {
        public string GameID { get; set; }
        public string Name { get; set; }
        public long IDNo { get; set; }
        public string PhoneNo { get; set; }
        public string Type { get { return "user"; } }
    }
}
