using System;

namespace Tutorial9.DTOs.Responses
{
    public class GetPatientResponseDTO
    {
        public int IdPatient { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthdate { get; set; }
    }
}
