using Assignment7.Models;
using Assignment7.Models.DTOs.Requests;
using Assignment7.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment7.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly IDatabaseService _dbService;

        public TripsController(IDatabaseService dbService)
        {
            _dbService = dbService;
        }


        [HttpGet]
        public async Task<IActionResult> GetTripsAsync()
        {
            var trips = await _dbService.GetTripsAsync();
            return Ok(trips);
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClientToTrip(AddClientsRequestDTO request, int idTrip)
        {
            bool response = await _dbService.AddClientToTourAsync(request, idTrip);

            if (!response)
            {
                return StatusCode(400, "Something went wrong");
            }

            return StatusCode(204);
        }
    }
}
