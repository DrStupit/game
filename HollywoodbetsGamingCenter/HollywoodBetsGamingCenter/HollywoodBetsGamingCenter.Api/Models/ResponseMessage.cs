using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetsGamingCenter.Api.Models
{
    public class ResponseMessage<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
