using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Service;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IDatabaseService _dbService;

        public FlightsController(IDatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{idPassenger}")]
        public async Task<IActionResult> GetFlightsAsync(int idPassenger)
        {
            var result = await _dbService.GetFlights(idPassenger);

            if (result is null)
            {
                return StatusCode(404, "Passenger with the given id is not in the database");
            };

            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> AddPlayerAsync(int idPassenger, int idFlight)
        {
            var passengerExists = await _dbService.PassengerExists(idPassenger);

            if (!passengerExists)
            {
                return StatusCode(404, "Passenger with the given id is not in database");
            }

            var flightExists = await _dbService.FlightExists(idFlight);

            if (!flightExists)
            {
                return StatusCode(404, "Flight with the given id is not in the database");
            };

            var passengerAssigned = await _dbService.PassengerAssigned(idPassenger, idFlight);

            if (passengerAssigned)
            {
                return StatusCode(400, "Passenger is already assigned to this flight");
            };

            _dbService.EnrollPassenger(idPassenger, idFlight);

            return StatusCode(204, "Passenger was added to flight");
        }
    }
}
