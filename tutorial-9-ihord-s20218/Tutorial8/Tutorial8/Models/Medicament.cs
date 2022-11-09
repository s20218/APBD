using System.Collections.Generic;

namespace Tutorial9.Models
{
    public class Medicament
    {
        public Medicament()
        {
            Prescription_Medicaments = new HashSet<Prescription_Medicament>();
        }

        public virtual ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; }
        public int IdMedicament { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }
    }
}
