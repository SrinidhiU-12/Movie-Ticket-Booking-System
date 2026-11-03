using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;

        public BookingService(IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<BookingResponse> CreateBookingAsync(
            CreateBookingRequest request)
        {
            if (request.SeatIds == null || request.SeatIds.Count == 0)
                throw new ArgumentException("Select at least one seat.");

            if (request.SeatIds.Distinct().Count() != request.SeatIds.Count)
                throw new ArgumentException("Duplicate seat IDs are not allowed.");

            await _repository.BeginTransactionAsync();

            try
            {
                var show = await _repository.GetShowByIdAsync(request.ShowId);

                if (show == null)
                    throw new KeyNotFoundException("Show not found.");

                var showSeats = await _repository.GetShowSeatsAsync(
                    request.ShowId, request.SeatIds);

                if (showSeats.Count != request.SeatIds.Count)
                    throw new ArgumentException(
                        "One or more selected seats do not belong to this show.");

                if (showSeats.Any(s => s.Status != "Available"))
                    throw new InvalidOperationException(
                        "One or more selected seats are already booked.");

                var reference = "BK" + Guid.NewGuid()
                    .ToString("N")[..20].ToUpperInvariant();

                var totalAmount = show.TicketPrice * request.SeatIds.Count;

                var booking = new Booking
                {
                    BookingReference = reference,
                    ShowId = request.ShowId,
                    CustomerName = request.CustomerName,
                    CustomerEmail = request.CustomerEmail,
                    CustomerPhone = request.CustomerPhone,
                    BookingDate = DateTime.UtcNow,
                    TotalAmount = totalAmount,
                    Status = "Confirmed"
                };

                await _repository.AddAsync(booking);
                await _repository.SaveChangesAsync();

                var bookingSeats = request.SeatIds.Select(seatId =>
                    new BookingSeat
                    {
                        BookingId = booking.BookingId,
                        SeatId = seatId,
                        Price = show.TicketPrice
                    }).ToList();

                await _repository.AddBookingSeatsAsync(bookingSeats);

                foreach (var seat in showSeats)
                    seat.Status = "Booked";

                await _repository.UpdateShowSeatsAsync(showSeats);
                await _repository.SaveChangesAsync();

                await _repository.CommitTransactionAsync();

                return MapToResponse(booking);
            }
            catch
            {
                await _repository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<BookingResponse?> GetBookingByIdAsync(int id)
        {
            var booking = await _repository.GetByIdAsync(id);

            return booking == null ? null : MapToResponse(booking);
        }

        public async Task<List<BookingResponse>> GetAllBookingsAsync(
            int? showId, string? status)
        {
            var bookings = await _repository.GetAllAsync(showId, status);

            return bookings.Select(MapToResponse).ToList();
        }

        public async Task<List<BookingResponse>> GetBookingsByShowIdAsync(
            int showId)
        {
            var bookings = await _repository.GetByShowIdAsync(showId);

            return bookings.Select(MapToResponse).ToList();
        }

        public async Task<BookingResponse?> CancelBookingAsync(int id)
        {
            await _repository.BeginTransactionAsync();

            try
            {
                var booking = await _repository.GetByIdAsync(id);

                if (booking == null)
                {
                    await _repository.RollbackTransactionAsync();
                    return null;
                }

                if (booking.Status == "Cancelled")
                    throw new InvalidOperationException(
                        "This booking is already cancelled.");

                var bookingSeats =
                    await _repository.GetBookingSeatsAsync(id);

                var seatIds = bookingSeats.Select(bs => bs.SeatId).ToList();

                var showSeats = await _repository.GetShowSeatsAsync(
                    booking.ShowId, seatIds);

                booking.Status = "Cancelled";

                foreach (var seat in showSeats)
                    seat.Status = "Available";

                await _repository.UpdateShowSeatsAsync(showSeats);
                await _repository.SaveChangesAsync();

                await _repository.CommitTransactionAsync();

                return MapToResponse(booking);
            }
            catch
            {
                await _repository.RollbackTransactionAsync();
                throw;
            }
        }

        private static BookingResponse MapToResponse(Booking booking)
        {
            return new BookingResponse
            {
                BookingId = booking.BookingId,
                BookingReference = booking.BookingReference,
                ShowId = booking.ShowId,
                CustomerName = booking.CustomerName,
                CustomerEmail = booking.CustomerEmail,
                CustomerPhone = booking.CustomerPhone,
                BookingDate = booking.BookingDate,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status
            };
        }
    }
}