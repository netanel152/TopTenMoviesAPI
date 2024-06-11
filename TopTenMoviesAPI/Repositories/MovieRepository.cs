using Microsoft.EntityFrameworkCore;
using TopTenMoviesAPI.Context;
using TopTenMoviesAPI.Context.Entities;
using TopTenMoviesAPI.Enums;
using TopTenMoviesAPI.Models.Dto;
using TopTenMoviesAPI.Repositories.Interfaces;

namespace TopTenMoviesAPI.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly MoviesDBContext _context;
    private readonly ILogger<MovieRepository> _logger;
    public MovieRepository(MoviesDBContext context, ILogger<MovieRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Movie>> GetMoviesByFilter(FilterMovieDto filterMovieDto)
    {
        IQueryable<Movie> query = _context.Movies.AsQueryable();

        switch (filterMovieDto.SearchType)
        {
            case SearchTypeEnum.Rate:
                query = query.OrderByDescending(movie => movie.Rate);
                break;

            case SearchTypeEnum.Category:
                if (!string.IsNullOrEmpty(filterMovieDto.CategoryValue))
                {
                    query = query.Where(w => w.Category == filterMovieDto.CategoryValue);
                }
                break;

            case SearchTypeEnum.Title:
                if (!string.IsNullOrEmpty(filterMovieDto.SearchValue))
                {
                    query = query.Where(w => w.Title.ToLower().Contains(filterMovieDto.SearchValue.ToLower()));
                }
                break;

            default:
                _logger.LogError($"{nameof(MovieRepository)} => {nameof(GetMoviesByFilter)} => Message: Missing filter movie dto");
                break;
        }

        query = query.Skip(filterMovieDto.Skip).Take(filterMovieDto.Take);

        return await query.ToListAsync();
    }

    public async Task<Movie?> GetSingleMovie(int id)
    {
        return await _context.Movies.FirstOrDefaultAsync(movie => movie.Id == id);
    }

    public async Task<int> UpdateMovie(Movie movie)
    {
        _context.Movies.Update(movie);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteMovie(int movieId)
    {
        var movie = await _context.Movies.FindAsync(movieId);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            return await _context.SaveChangesAsync();
        }

        return 0;
    }

    public async Task<int> CreateMovie(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        return await _context.SaveChangesAsync();
    }

    public async Task<Movie?> GetSingleMovieByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            _logger.LogWarning($"{nameof(MovieRepository)} => {nameof(GetSingleMovieByTitle)} => Message: Title is null or empty");
            return null;
        }
        return await _context.Movies.FirstOrDefaultAsync(movie => movie.Title == title);
    }

    public async Task<Movie?> GetMovieWithLowestRate()
    {
        return await _context.Movies.OrderBy(movie => movie.Rate).FirstOrDefaultAsync();
    }
}
