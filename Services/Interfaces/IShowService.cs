using MovieTicketBooking.DTOs.Shows;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IShowService
    {
        Task<List<ShowResponse>> GetAllShowsAsync(
            DateTime? date,
            int? movieId,
            int? theatreId);

        Task<ShowResponse?> GetShowByIdAsync(int id);

        Task<List<ShowResponse>> GetShowsByMovieIdAsync(int movieId);

        Task<List<ShowResponse>> GetShowsByTheatreIdAsync(int theatreId);

        Task<ShowResponse> CreateShowAsync(CreateShowRequest request);

        Task<ShowResponse?> UpdateShowAsync(
            int id,
            UpdateShowRequest request);

        Task<bool> DeleteShowAsync(int id);
    }
}