
namespace TopTenMoviesAPI.Models.Dto
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public double Rate { get; set; }
        public string ImagePath { get; set; }
    }
}
