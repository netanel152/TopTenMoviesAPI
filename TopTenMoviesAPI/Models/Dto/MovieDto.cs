﻿
namespace TopTenMoviesAPI.Models.Dto
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public float Rate { get; set; }
        public string Picture { get; set; }
    }
}