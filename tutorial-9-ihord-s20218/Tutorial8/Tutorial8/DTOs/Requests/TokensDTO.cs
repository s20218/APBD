using System.ComponentModel.DataAnnotations;

namespace Tutorial9.DTOs.Requests
{
    public class TokensDTO
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
