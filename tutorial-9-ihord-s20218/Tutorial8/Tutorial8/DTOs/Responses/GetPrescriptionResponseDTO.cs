using System;
using System.Collections.Generic;

namespace Tutorial9.DTOs.Responses
{
    public class GetPrescriptionResponseDTO
    {
        public int IdPrescription { get; set; }

        public DateTime Date { get; set; }

        public DateTime DueDate { get; set; }

        public GetPatientResponseDTO Patient { get; set; }

        public GetDoctorResponseDTO Doctor { get; set; }

        public IEnumerable<GetPrescriptions_MedicamentsResponseDTO> Prescriptions_Medicaments { get; set; }
    }
}
