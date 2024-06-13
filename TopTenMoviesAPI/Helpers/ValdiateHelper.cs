using TopTenMoviesAPI.Models.Dto;

namespace TopTenMoviesAPI.Helpers
{
    public static class ValdiateHelper
    {
        public static (bool IsValid, string Message) ValidateMovieDto(BaseMovieDto baseMovieDto)
        {
            if (baseMovieDto == null)
            {
                return (false, Constants.MovieObjectIsNull);

            }

            if (baseMovieDto.Rate < Constants.MinRate || baseMovieDto.Rate > Constants.MaxRate)
            {
                return (false, Constants.MovieRateInvalidMessage);
            }

            if (string.IsNullOrWhiteSpace(baseMovieDto.Title) ||
                string.IsNullOrWhiteSpace(baseMovieDto.Category) ||
                string.IsNullOrEmpty(baseMovieDto.ImagePath))
            {
                return (false, Constants.MovieTitleOrCategoryRequiredMessage);
            }

            return (true, string.Empty);
        }
    }
}
