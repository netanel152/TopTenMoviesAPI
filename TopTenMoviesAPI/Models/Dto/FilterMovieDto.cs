using TopTenMoviesAPI.Enums;

namespace TopTenMoviesAPI.Models.Dto;

public class FilterMovieDto
{

    public int Take { get; set; }
    public int Skip { get; set; }
    public string? SearchValue { get; set; }
    public string? CategoryValue { get; set; }
    public SearchTypeEnum SearchType { get; set; }
   
}
