using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetsGamingCenter.Api.Models
{
    public class Log
    {
        public string LogID { get; set; }
        public string MethodName { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public string Type { get { return "log"; } }
    }
}
