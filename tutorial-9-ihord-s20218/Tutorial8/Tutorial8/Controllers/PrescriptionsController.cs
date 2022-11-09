using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tutorial9.Services;

namespace Tutorial9.Controllers
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
