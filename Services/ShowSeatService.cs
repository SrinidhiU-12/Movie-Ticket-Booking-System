using MovieTicketBooking.DTOs.ShowSeats;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class ShowSeatService : IShowSeatService
    {
        private readonly IShowSeatRepository _showSeatRepository;

        public ShowSeatService(IShowSeatRepository showSeatRepository)
        {
            _showSeatRepository = showSeatRepository;
        }

        public async Task<List<ShowSeatResponse>?> GetAllSeatsAsync(int showId)
        {
            if (!await _showSeatRepository.ShowExistsAsync(showId))
                return null;

            var seats = await _showSeatRepository.GetByShowIdAsync(showId);

            return seats.Select(MapToResponse).ToList();
        }

        public async Task<List<ShowSeatResponse>?> GetAvailableSeatsAsync(int showId)
        {
            var seats = await GetAllSeatsAsync(showId);

            return seats?.Where(s => s.Status == "Available").ToList();
        }

        public async Task<List<ShowSeatResponse>?> GetBookedSeatsAsync(int showId)
        {
            var seats = await GetAllSeatsAsync(showId);

            return seats?.Where(s => s.Status == "Booked").ToList();
        }

        private static ShowSeatResponse MapToResponse(
            MovieTicketBooking.Models.ShowSeat showSeat)
        {
            return new ShowSeatResponse
            {
                ShowSeatId = showSeat.ShowSeatId,
                ShowId = showSeat.ShowId,
                SeatId = showSeat.SeatId,
                Status = showSeat.Status,
                RowName = showSeat.Seat?.RowName ?? string.Empty,
                SeatNumber = showSeat.Seat?.SeatNumber ?? 0,
                SeatType = showSeat.Seat?.SeatType ?? string.Empty
            };
        }
    }
}