
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<List<SeatResponse>> GetSeatsByScreenIdAsync(int screenId)
        {
            var seats = await _seatRepository.GetByScreenIdAsync(screenId);

            return seats.Select(s => MapToResponse(s)).ToList();
        }

        public async Task<SeatResponse?> GetSeatByIdAsync(int id)
        {
            var seat = await _seatRepository.GetByIdAsync(id);

            if (seat == null)
                return null;

            return MapToResponse(seat);
        }

        public async Task<SeatResponse> CreateSeatAsync(CreateSeatRequest request)
        {
            var seat = new Seat
            {
                ScreenId = request.ScreenId,
                RowName = request.RowName,
                SeatNumber = request.SeatNumber,
                SeatType = request.SeatType
            };

            await _seatRepository.AddAsync(seat);
            await _seatRepository.SaveChangesAsync();

            return MapToResponse(seat);
        }

        public async Task<List<SeatResponse>> BulkCreateSeatsAsync(BulkCreateSeatsRequest request)
        {
            var seats = request.Seats.Select(s => new Seat
            {
                ScreenId = request.ScreenId,
                RowName = s.RowName,
                SeatNumber = s.SeatNumber,
                SeatType = s.SeatType
            }).ToList();

            await _seatRepository.AddRangeAsync(seats);
            await _seatRepository.SaveChangesAsync();

            return seats.Select(s => MapToResponse(s)).ToList();
        }

        public async Task<SeatResponse?> UpdateSeatAsync(int id, UpdateSeatRequest request)
        {
            var seat = await _seatRepository.GetByIdAsync(id);

            if (seat == null)
                return null;

            seat.ScreenId = request.ScreenId;
            seat.RowName = request.RowName;
            seat.SeatNumber = request.SeatNumber;
            seat.SeatType = request.SeatType;

            await _seatRepository.SaveChangesAsync();

            return MapToResponse(seat);
        }

        public async Task<bool> DeleteSeatAsync(int id)
        {
            var seat = await _seatRepository.GetByIdAsync(id);

            if (seat == null)
                return false;

            await _seatRepository.DeleteAsync(seat);
            await _seatRepository.SaveChangesAsync();

            return true;
        }

        private static SeatResponse MapToResponse(Seat seat)
        {
            return new SeatResponse
            {
                SeatId = seat.SeatId,
                ScreenId = seat.ScreenId,
                RowName = seat.RowName,
                SeatNumber = seat.SeatNumber,
                SeatType = seat.SeatType
            };
        }
    }
}
