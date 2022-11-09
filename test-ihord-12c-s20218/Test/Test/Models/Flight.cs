using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Flight
    {
        public Flight()
        {
            Flight_Passengers = new HashSet<Flight_Passenger>();
        }

        public virtual ICollection<Flight_Passenger> Flight_Passengers { get; set; }
        public int IdFlight{ get; set; }

        public DateTime FlightDate { get; set; }

        public string Comment { get; set; }
        public int IdPlane { get; set; }
        public int IdCItyDict { get; set; }

        public virtual Plane IdPlaneNavigation { get; set; }
        public virtual CityDict IdCityDictNavigation { get; set; }
    }
}

