
namespace TopTenMoviesAPI.Models.Dto
{
    public class MovieDto : BaseMovieDto
    {
        public int Id { get; set; }
        public string? ImagePath { get; set; }
    }
}
