
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly ApplicationDbContext _context;

        public SeatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Seat>> GetByScreenIdAsync(int screenId)
        {
            return await _context.Seats
                .AsNoTracking()
                .Where(s => s.ScreenId == screenId)
                .OrderBy(s => s.RowName)
                .ThenBy(s => s.SeatNumber)
                .ToListAsync();
        }

        public async Task<Seat?> GetByIdAsync(int id)
        {
            return await _context.Seats
                .FirstOrDefaultAsync(s => s.SeatId == id);
        }

        public async Task AddAsync(Seat seat)
        {
            await _context.Seats.AddAsync(seat);
        }

        public async Task AddRangeAsync(List<Seat> seats)
        {
            await _context.Seats.AddRangeAsync(seats);
        }

        public async Task DeleteAsync(Seat seat)
        {
            _context.Seats.Remove(seat);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
