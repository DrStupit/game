using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetsGamingCenter.Api.Models
{
    public class ResultModel
    {
        public ResultModel()
        {
            User = new User();
        }

        public string ResultID { get; set; }
        public string GameID { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; }
        public string Type { get { return "result"; } }
    }
}
