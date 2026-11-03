using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IShowRepository
    {
        Task<List<Show>> GetAllAsync(
            DateTime? date,
            int? movieId,
            int? theatreId);

        Task<Show?> GetByIdAsync(int id);

        Task<List<Show>> GetByMovieIdAsync(int movieId);

        Task<List<Show>> GetByTheatreIdAsync(int theatreId);

        Task AddAsync(Show show);

        Task UpdateAsync(Show show);

        Task DeleteAsync(Show show);

        Task SaveChangesAsync();
    }
}