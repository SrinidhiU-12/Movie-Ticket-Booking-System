using MovieTicketBooking.DTOs.Bookings;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponse?> GetBookingByIdAsync(int id);

        Task<List<BookingResponse>> GetAllBookingsAsync(
            int? showId,
            string? status);

        Task<List<BookingResponse>> GetBookingsByShowIdAsync(int showId);

        Task<BookingResponse> CreateBookingAsync(
            CreateBookingRequest request);

        Task<BookingResponse?> CancelBookingAsync(int id);
    }
}