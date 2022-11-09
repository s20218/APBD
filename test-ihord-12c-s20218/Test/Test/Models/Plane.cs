using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Plane
    {
        public Plane()
        {
            Flights = new HashSet<Flight>();
        }

        public virtual ICollection<Flight> Flights { get; set; }
        public int IdPlane { get; set; }
        public string Name { get; set; }
        public int MaxSeats { get; set; }
    }
}
