namespace TopTenMoviesAPI.Models.Dto
{
    public class CreateMovieDto
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public double Rate { get; set; }
        public IFormFile? ImagePath { get; set; }
    }
}