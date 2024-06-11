using BacsoftLWFWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TopTenMoviesAPI.Context.Entities;
using TopTenMoviesAPI.Models.Dto;
using TopTenMoviesAPI.Services.Interfaces;

namespace TopTenMoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMovieService _moviesService;

        public MovieController(ILogger<MovieController> logger, IMovieService moveService)
        {
            _moviesService = moveService;
            _logger = logger;
        }

        [HttpPost("/all-movies")]
        public async Task<ActionResult<APIResponse<List<Movie>>>> Post(FilterMovieDto filterMovieDto)
        {
            _logger.LogInformation($"{nameof(MovieController)} => {nameof(Post)} => Message: Starting POST Request /all-movies");
            APIResponse<List<Movie>> _response = new();

            try
            {
                List<Movie> movies = await _moviesService.GetMoviesByFilter(filterMovieDto);
                if (movies.Count == 0)
                {
                    _response.IsSuccess = true;
                    _response.Data = [];
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.ErrorMessage = "No movies exist to retrieve";
                    return NotFound(_response);
                }
                _response.IsSuccess = true;
                _response.Data = movies;
                _response.StatusCode = HttpStatusCode.OK;
                _response.ErrorMessage = "";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = ex.Message;
                _logger.LogError($"{nameof(MovieController)} => {nameof(Post)} => Message: Exception with data: {_response.ErrorMessage}");
                return BadRequest(_response);
            }
        }

        [HttpPut("/update-movie")]
        public async Task<ActionResult<APIResponse<Movie>>> Put([FromForm] MovieDto movieDto)
        {
            _logger.LogInformation($"{nameof(MovieController)} => {nameof(Post)} => Message: Starting PUT Request /update-movie");
            APIResponse<Movie> _response = new();

            try
            {
                Movie updatedMovie = await _moviesService.UpdateMovie(movieDto);
                if (updatedMovie is null)
                {
                    _response.IsSuccess = false;
                    _response.Data = null;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessage = "";
                }

                _response.IsSuccess = true;
                _response.Data = updatedMovie;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = ex.Message;
                _logger.LogError($"{nameof(MovieController)} => {nameof(Put)} => Message: Exception with data: {_response.ErrorMessage}");
                return BadRequest(_response);
            }
        }

        [HttpPost("/create-movie")]
        public async Task<ActionResult<APIResponse<Movie>>> Post([FromForm] CreateMovieDto createMovieDto)
        {
            _logger.LogInformation($"{nameof(MovieController)} => {nameof(Post)} => Message: Starting POST Request /create-movie");
            APIResponse<Movie> _response = new();

            try
            {
                Movie? createdMovie = await _moviesService.CreateMovie(createMovieDto);

                if (createdMovie == null)
                {
                    _logger.LogWarning($"{nameof(MovieController)} => {nameof(Post)} => Failed to create the movie");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessage = "Failed to create the movie";
                    return BadRequest(_response);
                }

                _response.IsSuccess = true;
                _response.Data = createdMovie;
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = ex.Message;
                _logger.LogError(ex, $"{nameof(MovieController)} => {nameof(Post)} => Exception: {ex.Message}");
                return BadRequest(_response);
            }
        }
    }
}
