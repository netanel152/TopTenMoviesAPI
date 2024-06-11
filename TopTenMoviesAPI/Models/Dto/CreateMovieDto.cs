namespace TopTenMoviesAPI.Models.Dto
{
    public class CreateMovieDto : BaseMovieDto
    {
        public IFormFile? ImagePath { get; set; }
    }
}