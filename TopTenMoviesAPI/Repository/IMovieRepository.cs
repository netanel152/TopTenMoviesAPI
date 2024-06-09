using TopTenMoviesAPI.Models;
using TopTenMoviesAPI.Models.Dto;

namespace TopTenMoviesAPI.Repository
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetMovies();
        Task<Movie> CreateMovie(MovieDto movie);
        Task<Movie> UpdateMovie(MovieDto movie);
    }
}
