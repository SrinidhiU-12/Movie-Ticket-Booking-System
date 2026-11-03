
using MovieTicketBooking.DTOs.Seats;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface ISeatService
    {
        Task<List<SeatResponse>> GetSeatsByScreenIdAsync(int screenId);

        Task<SeatResponse?> GetSeatByIdAsync(int id);

        Task<SeatResponse> CreateSeatAsync(CreateSeatRequest request);

        Task<List<SeatResponse>> BulkCreateSeatsAsync(BulkCreateSeatsRequest request);

        Task<SeatResponse?> UpdateSeatAsync(int id, UpdateSeatRequest request);

        Task<bool> DeleteSeatAsync(int id);
    }
}
