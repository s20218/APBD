using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Shared.DTOs
{
    public class MovieAdderDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public bool InTheaters { get; set; }
        [Required]
        public string Trailer { get; set; }
        
        public DateTime? ReleaseDate { get; set; }
        [Required]
        public string Poster { get; set; }
    }
}
