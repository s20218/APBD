using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tutorial9.DTOs.Requests;
using Tutorial9.Services;

namespace Tutorial9.Controllers
{
    [Authorize]
    [Route("api/doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDatabaseService _dbService;

        public DoctorsController(IDatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{idDoctor}")]
        public async Task<IActionResult> GetDoctorsAsync(int idDoctor)
        {
            var doctor = await _dbService.GetDoctorsAsync(idDoctor);

            if(doctor is null)
            {
                return StatusCode(404, "Doctor with the given id is not in the database");
            }

            return Ok(doctor);
        }

        [HttpPost]
        public IActionResult AddDoctor(AddDoctorRequestDTO doctor)
        {
            bool result = _dbService.AddDoctor(doctor);
            return StatusCode(204);
        }

        [HttpPut("{idDoctor}")]
        public async Task<IActionResult> ModifyDoctorAsync(AddDoctorRequestDTO doctorRequest, int idDoctor)
        {
            bool result = await _dbService.ModifyDoctorAsync(doctorRequest, idDoctor);
            if (!result)
            {
                return StatusCode(404, "Doctor with the given id is not in the database");
            }

            return StatusCode(204);
        }

        [HttpDelete("{idDoctor}")]
        public async Task<IActionResult> DeleteDoctorAsync(int idDoctor)
        {
            bool result = await _dbService.DeleteDoctorAsync(idDoctor);

            if (!result)
            {
                return StatusCode(404, "Doctor with the given id is not in the database");
            }

            return StatusCode(204);
        }
    }
}
