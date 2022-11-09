using Assignment7.Models.DTO.Responses;
using Assignment7.Models.DTOs.Responses;
using System;
using System.Collections.Generic;

namespace Assignment7.Models.DTO
{
    public class GetTripsResponseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public IEnumerable<GetCountriesResponseDTO> Countries { get; set; }
        public IEnumerable<GetClientsResponseDTO> Clients { get; set; }

    }
}
