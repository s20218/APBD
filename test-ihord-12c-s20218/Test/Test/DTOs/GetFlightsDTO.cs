using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.DTOs
{
    public class GetFlightsDTO
    {
        public string NameOfCity { get; set; }
        public PlaneDTO Plane { get; set; }
    }
}
