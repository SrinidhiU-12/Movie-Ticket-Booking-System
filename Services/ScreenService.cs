
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IScreenRepository _screenRepository;

        public ScreenService(IScreenRepository screenRepository)
        {
            _screenRepository = screenRepository;
        }

        public async Task<List<ScreenResponse>> GetAllScreensAsync(int? theatreId)
        {
            var screens = await _screenRepository.GetAllAsync(theatreId);

            return screens.Select(s => MapToResponse(s)).ToList();
        }

        public async Task<ScreenResponse?> GetScreenByIdAsync(int id)
        {
            var screen = await _screenRepository.GetByIdAsync(id);

            if (screen == null)
                return null;

            return MapToResponse(screen);
        }

        public async Task<List<ScreenResponse>> GetScreensByTheatreIdAsync(int theatreId)
        {
            var screens = await _screenRepository.GetByTheatreIdAsync(theatreId);

            return screens.Select(s => MapToResponse(s)).ToList();
        }

        public async Task<ScreenResponse> CreateScreenAsync(CreateScreenRequest request)
        {
            var screen = new Screen
            {
                TheatreId = request.TheatreId,
                ScreenName = request.ScreenName
            };

            await _screenRepository.AddAsync(screen);
            await _screenRepository.SaveChangesAsync();

            return MapToResponse(screen);
        }

        public async Task<ScreenResponse?> UpdateScreenAsync(int id, UpdateScreenRequest request)
        {
            var screen = await _screenRepository.GetByIdAsync(id);

            if (screen == null)
                return null;

            screen.TheatreId = request.TheatreId;
            screen.ScreenName = request.ScreenName;

            await _screenRepository.SaveChangesAsync();

            return MapToResponse(screen);
        }

        public async Task<bool> DeleteScreenAsync(int id)
        {
            var screen = await _screenRepository.GetByIdAsync(id);

            if (screen == null)
                return false;

            await _screenRepository.DeleteAsync(screen);
            await _screenRepository.SaveChangesAsync();

            return true;
        }

        private static ScreenResponse MapToResponse(Screen screen)
        {
            return new ScreenResponse
            {
                ScreenId = screen.ScreenId,
                TheatreId = screen.TheatreId,
                ScreenName = screen.ScreenName
            };
        }
    }
}
