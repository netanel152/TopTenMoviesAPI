using Microsoft.EntityFrameworkCore;
using TopTenMoviesAPI.Data;
using TopTenMoviesAPI.Models;
using TopTenMoviesAPI.Models.Dto;

namespace TopTenMoviesAPI.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MoviesDBContext _context;
        public MovieRepository(MoviesDBContext context)
        {
            _context = context;
        }
        public async Task<List<Movie>> GetMovies()
        {
            List<Movie> movieList = await _context.Movies.ToListAsync();
            return movieList;
        }

        public Task<Movie> CreateMovie(MovieDto movie)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> UpdateMovie(MovieDto movie)
        {
            throw new NotImplementedException();
        }
    }
}
