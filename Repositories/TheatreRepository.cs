using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class TheatreRepository : ITheatreRepository
    {
        private readonly ApplicationDbContext _context;

        public TheatreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Theatre>> GetAllAsync(
            int pageNumber, int pageSize)
        {
            return await _context.Theatres
                .AsNoTracking()
                .OrderBy(t => t.TheatreId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Theatre?> GetByIdAsync(int id)
        {
            return await _context.Theatres
                .FirstOrDefaultAsync(t => t.TheatreId == id);
        }

        public async Task AddAsync(Theatre theatre)
        {
            await _context.Theatres.AddAsync(theatre);
        }

        public async Task DeleteAsync(Theatre theatre)
        {
            _context.Theatres.Remove(theatre);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}