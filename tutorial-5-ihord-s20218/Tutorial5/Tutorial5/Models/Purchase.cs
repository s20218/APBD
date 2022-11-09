using System;
using System.ComponentModel.DataAnnotations;

namespace Tutorial5.Models
{
    public class Purchase
    {
        [Required(ErrorMessage = "id of product cannot be null")]
        public int IdProduct { get; set; }

        [Required(ErrorMessage = "id of warehouse cannot be null")]
        public int IdWarehouse { get; set; }

        [Required(ErrorMessage = "amount of products cannot be null")]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }

        [Required(ErrorMessage = "date and time of creation cannot be null")]
        public DateTime CreatedAt { get; set; }

    }
}
