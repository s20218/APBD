using System.ComponentModel.DataAnnotations;

namespace Tutorial4_webApp.Models
{
    public class Animal
    {
        [Required(ErrorMessage = "id cannot be null")]
        public int IdAnimal { get; set; }

        [Required(ErrorMessage = "name cannot be null")]
        [MaxLength(200, ErrorMessage = "Length of name cannot exceed 200")]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "Length of description cannot exceed 200")]
        public string Description { get; set; }

        [Required(ErrorMessage = "category cannot be null")]
        [MaxLength(200, ErrorMessage = "Length of category cannot exceed 200")]
        public string Category { get; set; }

        [Required(ErrorMessage = "area cannot be null")]
        [MaxLength(200, ErrorMessage = "Length of area cannot exceed 200")]
        public string Area { get; set; }

    }
}
