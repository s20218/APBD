using System;
using System.Collections.Generic;

namespace Tutorial9.Models
{
    public class Patient
    {
        public Patient()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public int IdPatient { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthdate { get; set; }
    }
}
