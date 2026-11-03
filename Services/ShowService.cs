using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IScreenRepository _screenRepository;

        public ShowService(
            IShowRepository showRepository,
            IMovieRepository movieRepository,
            IScreenRepository screenRepository)
        {
            _showRepository = showRepository;
            _movieRepository = movieRepository;
            _screenRepository = screenRepository;
        }

        // Get all shows with optional filters
        public async Task<List<ShowResponse>> GetAllShowsAsync(
            DateTime? date,
            int? movieId,
            int? theatreId)
        {
            var shows = await _showRepository.GetAllAsync(
                date, movieId, theatreId);

            return shows.Select(MapToResponse).ToList();
        }

        // Get show by ID
        public async Task<ShowResponse?> GetShowByIdAsync(int id)
        {
            var show = await _showRepository.GetByIdAsync(id);

            return show == null ? null : MapToResponse(show);
        }

        // Get shows by movie ID
        public async Task<List<ShowResponse>> GetShowsByMovieIdAsync(
            int movieId)
        {
            var shows = await _showRepository.GetByMovieIdAsync(movieId);

            return shows.Select(MapToResponse).ToList();
        }

        // Get shows by theatre ID
        public async Task<List<ShowResponse>> GetShowsByTheatreIdAsync(
            int theatreId)
        {
            var shows = await _showRepository.GetByTheatreIdAsync(theatreId);

            return shows.Select(MapToResponse).ToList();
        }

        // Create show
        public async Task<ShowResponse> CreateShowAsync(
            CreateShowRequest request)
        {
            ValidateShowDetails(
                request.StartTime,
                request.EndTime,
                request.TicketPrice);

            var movie = await _movieRepository.GetByIdAsync(request.MovieId);

            if (movie == null)
                throw new ArgumentException("Movie not found.");

            var screen = await _screenRepository.GetByIdAsync(request.ScreenId);

            if (screen == null)
                throw new ArgumentException("Screen not found.");

            var show = new Show
            {
                MovieId = request.MovieId,
                ScreenId = request.ScreenId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                TicketPrice = request.TicketPrice
            };

            await _showRepository.AddAsync(show);
            await _showRepository.SaveChangesAsync();

            return MapToResponse(show);
        }

        // Update show
        public async Task<ShowResponse?> UpdateShowAsync(
            int id,
            UpdateShowRequest request)
        {
            ValidateShowDetails(
                request.StartTime,
                request.EndTime,
                request.TicketPrice);

            var show = await _showRepository.GetByIdAsync(id);

            if (show == null)
                return null;

            var movie = await _movieRepository.GetByIdAsync(request.MovieId);

            if (movie == null)
                throw new ArgumentException("Movie not found.");

            var screen = await _screenRepository.GetByIdAsync(request.ScreenId);

            if (screen == null)
                throw new ArgumentException("Screen not found.");

            show.MovieId = request.MovieId;
            show.ScreenId = request.ScreenId;
            show.StartTime = request.StartTime;
            show.EndTime = request.EndTime;
            show.TicketPrice = request.TicketPrice;

            await _showRepository.UpdateAsync(show);
            await _showRepository.SaveChangesAsync();

            return MapToResponse(show);
        }

        // Delete show
        public async Task<bool> DeleteShowAsync(int id)
        {
            var show = await _showRepository.GetByIdAsync(id);

            if (show == null)
                return false;

            await _showRepository.DeleteAsync(show);
            await _showRepository.SaveChangesAsync();

            return true;
        }

        // Validate show details
        private static void ValidateShowDetails(
            DateTime startTime,
            DateTime endTime,
            decimal ticketPrice)
        {
            if (endTime <= startTime)
                throw new ArgumentException(
                    "End time must be later than start time.");

            if (ticketPrice < 0)
                throw new ArgumentException(
                    "Ticket price cannot be negative.");
        }

        // Map entity to response DTO
        private static ShowResponse MapToResponse(Show show)
        {
            return new ShowResponse
            {
                ShowId = show.ShowId,
                MovieId = show.MovieId,
                ScreenId = show.ScreenId,
                StartTime = show.StartTime,
                EndTime = show.EndTime,
                TicketPrice = show.TicketPrice
            };
        }
    }
}