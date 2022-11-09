using Microsoft.AspNetCore.Mvc;
using Tutorial4_webApp.Models;
using Tutorial4_webApp.Services;

namespace Tutorial4_webApp.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private IDatabaseService _dbService;

        public AnimalsController(IDatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetAnimals([FromQuery] string orderBy = "name")
        {
            if (_dbService.GetAnimals(orderBy) is null)
                return BadRequest("Something went wrong");

            return Ok(_dbService.GetAnimals(orderBy));
        }

        [HttpPost]
        public IActionResult PostAnimal(Animal animal)
        {
            int code = _dbService.AddAnimal(animal);

            return StatusCode(code);
        }

        [HttpPut("{idAnimal}")]
        public IActionResult PutAnimal(int idAnimal, Animal animal)
        {
            int code = _dbService.ModifyAnimal(idAnimal, animal);

            return StatusCode(code);
        }

        [HttpDelete("{idAnimal}")]
        public IActionResult DeleteAnimal(int idAnimal)
        {
            int code = _dbService.DeleteAnimal(idAnimal);

            return StatusCode(code);
        }
    }
}
