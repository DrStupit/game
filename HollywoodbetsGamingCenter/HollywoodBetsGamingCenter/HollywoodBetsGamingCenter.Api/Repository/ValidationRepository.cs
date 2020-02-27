using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetsGamingCenter.Api.Repository
{
    public static class ValidationRepository
    {
        public static bool EighteenYearsOrOlder(int user_year)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - user_year;
            if (age >= 18)
                return true;
            else
                return false;
        }
    }
}
