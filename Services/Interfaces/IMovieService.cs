
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieResponse>> GetAllMoviesAsync(
            int pageNumber,
            int pageSize,
            string? title);

        Task<MovieResponse?> GetMovieByIdAsync(int id);

        Task<MovieResponse> CreateMovieAsync(
            CreateMovieRequest request);

        Task<MovieResponse?> UpdateMovieAsync(
            int id,
            UpdateMovieRequest request);

        Task<bool> DeleteMovieAsync(int id);

        Task<List<MovieResponse>> SearchMoviesAsync(string title);
        Task<MovieResponse?> PatchMovieAsync(int id, PatchMovieRequest request);
    }
}
