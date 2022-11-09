using System.ComponentModel.DataAnnotations;

namespace Tutorial9.DTOs.Requests
{
    public class AddDoctorRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
