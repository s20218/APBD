using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieApp.Server.Services;
using MovieApp.Shared.DTOs;
using MovieApp.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MovieApp.Server.Controllers
{
    [Authorize]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMoviesDbService _dbService;

        public MoviesController(ILogger<MoviesController> logger, IMoviesDbService dbService)
        {
            _logger = logger;
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            List<Movie> movies = await _dbService.GetMovies();

            if (movies != null)
            {
                return Ok(await _dbService.GetMovies());
            }
            return StatusCode(404);

        }

        [HttpGet("{idMovie}")]
        public async Task<IActionResult> GetMovie(int idMovie)
        {
            Movie movie = _dbService.GetMovie(idMovie);

            if (movie is null)
            {
                return StatusCode(404);
            }

            return Ok(_dbService.GetMovie(idMovie));

        }

        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] MovieAdderDto movie)
        {
            bool okay = await _dbService.AddMovie(movie);

            if (!okay)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{idMovie}")]
        public async Task<IActionResult> DeleteMovieAsync(int idMovie)
        {
            bool exists = await _dbService.DeleteMovie(idMovie);

            if (!exists)
            {
                return StatusCode(404, "Movie doesn't exist");
            }

            return Ok();
        }

        [HttpPut("{idMovie}")]
        public async Task<IActionResult> UpdateMovie(int idMovie, [FromBody] Movie movie)
        {
            bool exists = await _dbService.EditMovie(idMovie, movie);

            if (!exists)
            {
                return StatusCode(404, "Movie doesn't exist");
            }

            return Ok();
        }
    }
}
