
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all movies with pagination and optional title filtering
        public async Task<List<Movie>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? title)
        {
            var query = _context.Movies.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(m =>
                    m.Title.Contains(title));
            }

            return await query
                .OrderBy(m => m.MovieId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Get movie by ID
        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
        }

        // Search movies by title
        public async Task<List<Movie>> SearchByTitleAsync(string title)
        {
            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.Title.Contains(title))
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        // Add a new movie
        public async Task AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
        }

        // Delete an existing movie
        public Task DeleteAsync(Movie movie)
        {
            _context.Movies.Remove(movie);

            return Task.CompletedTask;
        }

        // Save changes to database
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
