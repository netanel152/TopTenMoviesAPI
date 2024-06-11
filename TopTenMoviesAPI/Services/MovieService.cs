using TopTenMoviesAPI.Context.Entities;
using TopTenMoviesAPI.Models.Dto;
using TopTenMoviesAPI.Repositories.Interfaces;
using TopTenMoviesAPI.Services.Interfaces;

namespace TopTenMoviesAPI.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly ILogger<MovieService> _logger;
    public MovieService(IMovieRepository movieRepository, ILogger<MovieService> logger)
    {
        _movieRepository = movieRepository;
        _logger = logger;
    }

    public async Task<List<Movie?>> GetMoviesByFilter(FilterMovieDto filterMovieDto)
    {
        if (filterMovieDto == null)
        {
            _logger.LogWarning($"{nameof(MovieService)} => {nameof(GetMoviesByFilter)} => Error: FilterMovieDto is null");
            return [];
        }

        try
        {
            List<Movie> movies = await _movieRepository.GetMoviesByFilter(filterMovieDto);
            return movies ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(MovieService)} => {nameof(GetMoviesByFilter)} => Exception: {ex.Message}");
            return [];
        }
    }


    public async Task<Movie?> CreateMovie(CreateMovieDto createMovieDto)
    {
        if (createMovieDto == null)
        {
            _logger.LogWarning($"{nameof(MovieService)} => {nameof(CreateMovie)} => Message: movieDto is null");
            return null;
        }

        try
        {
            Movie? existingMovie = await _movieRepository.GetSingleMovieByTitle(createMovieDto.Title);
            if (existingMovie != null)
            {
                _logger.LogWarning($"{nameof(MovieService)} => {nameof(CreateMovie)} => Message: Movie with title {createMovieDto.Title} already exists");
                throw new InvalidOperationException($"Movie with title '{createMovieDto.Title}' already exists.");
            }

            Movie? lowestRatedMovie = await _movieRepository.GetMovieWithLowestRate();
            if (lowestRatedMovie != null)
            {
                await _movieRepository.DeleteMovie(lowestRatedMovie.Id);
                _logger.LogInformation($"{nameof(MovieService)} => {nameof(CreateMovie)} => Message: Removed movie with ID {lowestRatedMovie.Id} having the lowest rate");
            }

            //string binPath = Path.Combine(Directory.GetCurrentDirectory(), "bin", "Debug", "net8.0", "Images");
            //string imagePath = Path.Combine(binPath, createMovieDto.ImagePath.FileName);
            //using (var stream = new FileStream(imagePath, FileMode.Create))
            //{
            //    await createMovieDto.ImagePath.CopyToAsync(stream);
            //}

            Movie newMovie = new()
            {
                Title = createMovieDto.Title,
                Category = createMovieDto.Category,
                Rate = createMovieDto.Rate
                //ImagePath = createMovieDto.ImagePath.FileName
            };

            int result = await _movieRepository.CreateMovie(newMovie);
            if (result <= 0)
            {
                _logger.LogWarning($"{nameof(MovieService)} => {nameof(CreateMovie)} => Message: Failed to create movie");
                return null;
            }

            return newMovie;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(MovieService)} => {nameof(CreateMovie)} => Exception: {ex.Message}");
            throw; // Rethrow the exception for the caller to handle
        }
    }

    public async Task<Movie?> UpdateMovie(MovieDto movieDto)
    {
        if (movieDto == null)
        {
            _logger.LogWarning($"{nameof(MovieService)} => {nameof(UpdateMovie)} => Message: movieDto is null");
            return null;
        }

        try
        {
            Movie? movie = await _movieRepository.GetSingleMovie(movieDto.Id);
            if (movie == null)
            {
                _logger.LogWarning($"{nameof(MovieService)} => {nameof(UpdateMovie)} => Message: Movie with ID {movieDto.Id} not found");
                return null;
            }

            movie.Title = movieDto.Title;
            movie.Category = movieDto.Category;
            movie.Rate = movieDto.Rate;
            movie.LastUpdatedDate = new DateTime(
                DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second);

            int result = await _movieRepository.UpdateMovie(movie);
            if (result <= 0)
            {
                _logger.LogWarning($"{nameof(MovieService)} => {nameof(UpdateMovie)} => Message: Failed to update movie with ID {movieDto.Id}");
                return null;
            }

            return movie;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(MovieService)} => {nameof(UpdateMovie)} => Exception: {ex.Message}");
            throw;
        }
    }
}
