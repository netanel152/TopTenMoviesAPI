using TopTenMoviesAPI.Context.Entities;
using TopTenMoviesAPI.Models.Dto;

namespace TopTenMoviesAPI.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<Movie?>> GetMoviesByFilter(FilterMovieDto filterMovieDto);
        Task<Movie?> CreateMovie(CreateMovieDto movie);
        Task<Movie?> UpdateMovie(MovieDto movie);
    }
}
