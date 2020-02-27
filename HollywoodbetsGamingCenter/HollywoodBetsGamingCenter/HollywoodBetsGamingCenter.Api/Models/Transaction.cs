using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetsGamingCenter.Api.Models
{
    public class Transaction
    {
        public Transaction()
        {
            User = new User();
            Prize = new Prize();
        }

        public string TransactionID { get; set; }
        public User User { get; set; }
        public Prize Prize { get; set; }
        public bool IsMonetary { get; set; }
        public DateTime Created { get; set; }
        public string Type { get { return "transaction"; } }
    }
}
