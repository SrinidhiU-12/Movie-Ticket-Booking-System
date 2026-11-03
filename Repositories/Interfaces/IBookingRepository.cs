using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);

        Task<List<Booking>> GetAllAsync(
            int? showId,
            string? status);

        Task<List<Booking>> GetByShowIdAsync(int showId);

        Task<Show?> GetShowByIdAsync(int showId);

        Task<List<ShowSeat>> GetShowSeatsAsync(
            int showId,
            List<int> seatIds);

        Task<List<BookingSeat>> GetBookingSeatsAsync(int bookingId);

        Task<bool> BookingReferenceExistsAsync(string reference);

        Task AddAsync(Booking booking);

        Task AddBookingSeatsAsync(List<BookingSeat> bookingSeats);

        Task UpdateShowSeatsAsync(List<ShowSeat> showSeats);

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        Task SaveChangesAsync();
    }
}