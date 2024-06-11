using TopTenMoviesAPI.Models.Dto;

namespace TopTenMoviesAPI.Helpers
{
    public static class ValdiateHelper
    {
        public static (bool IsValid, string Message) ValidateMovieDto(BaseMovieDto movieDto)
        {
            if (movieDto.Rate < Constants.MinRate || movieDto.Rate > Constants.MaxRate)
            {
                return (false, Constants.MovieRateInvalidMessage);
            }

            if (string.IsNullOrWhiteSpace(movieDto.Title) || string.IsNullOrWhiteSpace(movieDto.Category))
            {
                return (false, Constants.MovieTitleOrCategoryRequiredMessage);
            }

            return (true, string.Empty);
        }
    }
}
