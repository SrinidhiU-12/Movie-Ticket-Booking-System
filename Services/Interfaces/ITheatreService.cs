using MovieTicketBooking.DTOs.Theatres;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface ITheatreService
    {
        Task<List<TheatreResponse>> GetAllTheatresAsync(
            int pageNumber, int pageSize);

        Task<TheatreResponse?> GetTheatreByIdAsync(int id);
        
        Task<TheatreResponse> CreateTheatreAsync(
            CreateTheatreRequest request);

        Task<TheatreResponse?> UpdateTheatreAsync(
            int id, UpdateTheatreRequest request);

        Task<bool> DeleteTheatreAsync(int id);
    }
}