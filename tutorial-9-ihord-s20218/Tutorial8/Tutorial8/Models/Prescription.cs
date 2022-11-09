using System;
using System.Collections.Generic;

namespace Tutorial9.Models
{
    public class Prescription
    {
        public Prescription()
        {
            Prescription_Medicaments = new HashSet<Prescription_Medicament>();
        }

        public virtual ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; }
        public int IdPrescription { get; set; }

        public DateTime Date { get; set; }

        public DateTime DueDate { get; set; }

        public int IdPatient { get; set; }

        public int IdDoctor { get; set; }

        public virtual Patient IdPatientNavigation { get; set; }
        public virtual Doctor IdDoctorNavigation { get; set; }

    }
}
