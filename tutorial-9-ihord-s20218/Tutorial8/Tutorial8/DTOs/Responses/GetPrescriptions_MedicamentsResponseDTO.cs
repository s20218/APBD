
namespace Tutorial9.DTOs.Responses
{
    public class GetPrescriptions_MedicamentsResponseDTO
    {
        public int? Dose { get; set; }

        public string Details { get; set; }

        public GetMedicamentsResponse Medicament { get; set; }
    }
}
