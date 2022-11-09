using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [Route("api/prescriptions")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IDatabaseService _dbService;

        public PrescriptionsController(IDatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{idPrescription}")]
        public async Task<IActionResult> GetPrescriptionsAsync(int idPrescription)
        {
            var prescription = await _dbService.GetPrescriptionAsync(idPrescription);

            if (prescription is null)
            {
                return StatusCode(404, "Prescription with the given id is not in the database");
            }

            return Ok(prescription);
        }
    }
}
