using Microsoft.EntityFrameworkCore;
using MovieApp.Server.Data;
using MovieApp.Shared.Models;
using MovieApp.Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Server.Services
{
    public interface IMoviesRepository
    {

    }

    public interface IMoviesDbService
    {
        Task<List<Movie>> GetMovies();
        Movie GetMovie(int idMovie);
        Task<bool> AddMovie(MovieAdderDto movie);
        Task<bool> DeleteMovie(int idMovie);
        Task<bool> EditMovie(int idMovie, Movie movieDTO);
    }

    public class MoviesDbService : IMoviesDbService
    {
        private ApplicationDbContext _context;

        public MoviesDbService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddMovie(MovieAdderDto movie)
        {
            if(movie.Title is null || movie.Summary is null || movie.Trailer is null || movie.ReleaseDate is null || movie.Poster is null)
            {
                return false;
            }

            await _context.Movies.AddAsync(new Movie
            {
                Title = movie.Title,
                Summary = movie.Summary,
                InTheaters = movie.InTheaters,
                Trailer = movie.Trailer,
                ReleaseDate = movie.ReleaseDate,
                Poster = movie.Poster
            });

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteMovie(int idMovie)
        {
            Movie movie = await _context.Movies.Where(m => m.Id == idMovie).FirstOrDefaultAsync();
            if(movie is null)
            {
                return false;
            }

            _context.Remove(movie);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> EditMovie(int idMovie, Movie movieDTO)
        {

            Movie movie = await _context.Movies.Where(m => m.Id == idMovie).FirstOrDefaultAsync();

            if (movie is null)
            {
                return false;
            }

            movie.Title = movieDTO.Title;
            movie.Summary = movieDTO.Summary;
            movie.InTheaters = movie.InTheaters;
            movie.Trailer = movie.Trailer;
            movie.ReleaseDate = movie.ReleaseDate;
            movie.Poster = movie.Poster;
            await _context.SaveChangesAsync();

            return true;

        }

        public Movie GetMovie(int idMovie)
        {
            Movie movie = _context.Movies.Where(m => m.Id == idMovie).FirstOrDefault();

            return movie;
        }

        public Task<List<Movie>> GetMovies()
        {
            return _context.Movies.OrderBy(m => m.Title).ToListAsync();
        }
    }
}
