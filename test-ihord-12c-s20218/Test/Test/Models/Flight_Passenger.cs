using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Flight_Passenger
    {
        public int IdFlight { get; set; }
        public int IdPassenger { get; set; }

        public virtual Flight IdFlightNavigation { get; set; }
        public virtual Passenger IdPassengerNavigation { get; set; }
    }
}
