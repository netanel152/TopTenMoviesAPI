using BacsoftLWFWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TopTenMoviesAPI.Models;
using TopTenMoviesAPI.Repository;

namespace TopTenMoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMovieRepository _movieRepository;

        public MovieController(ILogger<MovieController> logger, IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }

        [HttpGet("/all-movies")]
        public async Task<ActionResult<APIResponse<List<Movie>>>> Get()
        {
            _logger.LogInformation($"Starting GET Request api/all-movies");
            APIResponse<List<Movie>> _response = new();

            try
            {
                List<Movie> movies = await _movieRepository.GetMovies();
                if (movies.Count == 0)
                {
                    _response.IsSuccess = true;
                    _response.Data = [];
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.ErrorMessages = new List<string> { "No movies exist to retrieve" };
                    return NotFound(_response);
                }
                _response.IsSuccess = true;
                _response.Data = movies;
                _response.StatusCode = HttpStatusCode.OK;
                _response.ErrorMessages = new List<string>();
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                _logger.LogError(message: $"GET request ==> Exception with data: {_response.ErrorMessages}");
                return BadRequest(_response);
            }


        }
    }
}
