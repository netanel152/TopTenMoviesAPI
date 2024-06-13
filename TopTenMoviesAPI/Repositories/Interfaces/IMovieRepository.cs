using TopTenMoviesAPI.Context.Entities;
using TopTenMoviesAPI.Models.Dto;

namespace TopTenMoviesAPI.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetMoviesByFilter(FilterMovieDto filterMovieDto);
    Task<Movie?> GetSingleMovie(Guid id);
    Task<Movie?> GetSingleMovieByTitle(string title);
    Task<Movie?> GetMovieWithLowestRate();
    Task<int> CreateMovie(Movie newMovie);
    Task<int> UpdateMovie(Movie movie);
    Task<int> DeleteMovie(Guid id);
}
