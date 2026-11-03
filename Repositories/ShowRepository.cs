using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all shows with optional filters
        public async Task<List<Show>> GetAllAsync(
            DateTime? date,
            int? movieId,
            int? theatreId)
        {
            var query = _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .AsQueryable();

            if (date.HasValue)
            {
                var startOfDay = date.Value.Date;
                var endOfDay = startOfDay.AddDays(1);

                query = query.Where(s =>
                    s.StartTime >= startOfDay &&
                    s.StartTime < endOfDay);
            }

            if (movieId.HasValue)
            {
                query = query.Where(s => s.MovieId == movieId.Value);
            }

            if (theatreId.HasValue)
            {
                query = query.Where(s =>
                    s.Screen != null &&
                    s.Screen.TheatreId == theatreId.Value);
            }

            return await query
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        // Get show by ID
        public async Task<Show?> GetByIdAsync(int id)
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .FirstOrDefaultAsync(s => s.ShowId == id);
        }

        // Get shows by movie ID
        public async Task<List<Show>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Shows
                .Where(s => s.MovieId == movieId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        // Get shows by theatre ID
        public async Task<List<Show>> GetByTheatreIdAsync(int theatreId)
        {
            return await _context.Shows
                .Where(s => s.Screen != null &&
                            s.Screen.TheatreId == theatreId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        // Add show
        public async Task AddAsync(Show show)
        {
            await _context.Shows.AddAsync(show);
        }

        // Update show
        public async Task UpdateAsync(Show show)
        {
            _context.Shows.Update(show);
        }

        // Delete show
        public async Task DeleteAsync(Show show)
        {
            _context.Shows.Remove(show);
        }

        // Save changes
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
