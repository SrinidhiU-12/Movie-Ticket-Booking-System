
using MovieTicketBooking.DTOs.Screens;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IScreenService
    {
        Task<List<ScreenResponse>> GetAllScreensAsync(int? theatreId);

        Task<ScreenResponse?> GetScreenByIdAsync(int id);

        Task<List<ScreenResponse>> GetScreensByTheatreIdAsync(int theatreId);

        Task<ScreenResponse> CreateScreenAsync(CreateScreenRequest request);

        Task<ScreenResponse?> UpdateScreenAsync(int id, UpdateScreenRequest request);

        Task<bool> DeleteScreenAsync(int id);
    }
}
