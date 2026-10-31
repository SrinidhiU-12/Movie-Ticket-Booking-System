
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? title);

        Task<Movie?> GetByIdAsync(int id);

        Task<List<Movie>> SearchByTitleAsync(string title);

        Task AddAsync(Movie movie);

        Task DeleteAsync(Movie movie);

        Task SaveChangesAsync();
    }
}
