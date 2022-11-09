using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial8.DTOs.Responses
{
    public class GetPrescriptions_MedicamentsResponseDTO
    {
        public int? Dose { get; set; }

        public string Details { get; set; }

        public GetMedicamentsResponse Medicament { get; set; }
    }
}
