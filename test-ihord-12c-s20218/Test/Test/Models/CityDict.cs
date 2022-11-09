using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class CityDict
    {
        public CityDict()
        {
            Flights = new HashSet<Flight>();
        }

        public virtual ICollection<Flight> Flights { get; set; }
        public int IdCityDict { get; set; }
        public string City { get; set; }
    }
}
