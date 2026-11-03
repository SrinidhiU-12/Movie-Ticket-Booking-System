using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Show)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task<List<Booking>> GetAllAsync(
            int? showId, string? status)
        {
            var query = _context.Bookings
                .AsQueryable();

            if (showId.HasValue)
                query = query.Where(b => b.ShowId == showId.Value);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(b => b.Status == status);

            return await query
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetByShowIdAsync(int showId)
        {
            return await _context.Bookings
                .Where(b => b.ShowId == showId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<Show?> GetShowByIdAsync(int showId)
        {
            return await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == showId);
        }

        public async Task<List<ShowSeat>> GetShowSeatsAsync(
            int showId, List<int> seatIds)
        {
            return await _context.ShowSeats
                .Where(ss => ss.ShowId == showId &&
                             seatIds.Contains(ss.SeatId))
                .ToListAsync();
        }

        public async Task<List<BookingSeat>> GetBookingSeatsAsync(
            int bookingId)
        {
            return await _context.BookingSeats
                .Where(bs => bs.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task<bool> BookingReferenceExistsAsync(
            string reference)
        {
            return await _context.Bookings
                .AnyAsync(b => b.BookingReference == reference);
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public async Task AddBookingSeatsAsync(
            List<BookingSeat> bookingSeats)
        {
            await _context.BookingSeats.AddRangeAsync(bookingSeats);
        }

        public async Task UpdateShowSeatsAsync(
            List<ShowSeat> showSeats)
        {
            _context.ShowSeats.UpdateRange(showSeats);
            await Task.CompletedTask;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction =
                await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}