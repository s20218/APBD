using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Passenger
    {
        public Passenger()
        {
            Flight_Passengers = new HashSet<Flight_Passenger>();
        }

        public virtual ICollection<Flight_Passenger> Flight_Passengers { get; set; }
        public int IdPassenger { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassportNum { get; set; }
    }
}
