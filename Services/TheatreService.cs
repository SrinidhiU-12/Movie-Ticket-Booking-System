using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class TheatreService : ITheatreService
    {
        private readonly ITheatreRepository _theatreRepository;

        public TheatreService(ITheatreRepository theatreRepository)
        {
            _theatreRepository = theatreRepository;
        }

        // Get all theatres
        public async Task<List<TheatreResponse>> GetAllTheatresAsync(
            int pageNumber, int pageSize)
        {
            var theatres = await _theatreRepository.GetAllAsync(
                pageNumber, pageSize);

            return theatres.Select(t => MapToResponse(t)).ToList();
        }

        // Get theatre by ID
        public async Task<TheatreResponse?> GetTheatreByIdAsync(int id)
        {
            var theatre = await _theatreRepository.GetByIdAsync(id);

            if (theatre == null)
                return null;

            return MapToResponse(theatre);
        }

        // Create theatre
        public async Task<TheatreResponse> CreateTheatreAsync(
            CreateTheatreRequest request)
        {
            var theatre = new Theatre
            {
                Name = request.Name,
                Location = request.Location
            };

            await _theatreRepository.AddAsync(theatre);
            await _theatreRepository.SaveChangesAsync();

            return MapToResponse(theatre);
        }

        // Update theatre
        public async Task<TheatreResponse?> UpdateTheatreAsync(
            int id, UpdateTheatreRequest request)
        {
            var theatre = await _theatreRepository.GetByIdAsync(id);

            if (theatre == null)
                return null;

            theatre.Name = request.Name;
            theatre.Location = request.Location;

            await _theatreRepository.SaveChangesAsync();

            return MapToResponse(theatre);
        }

        // Delete theatre
        public async Task<bool> DeleteTheatreAsync(int id)
        {
            var theatre = await _theatreRepository.GetByIdAsync(id);

            if (theatre == null)
                return false;

            await _theatreRepository.DeleteAsync(theatre);
            await _theatreRepository.SaveChangesAsync();

            return true;
        }

        // Convert Theatre entity into TheatreResponse DTO
        private static TheatreResponse MapToResponse(Theatre theatre)
        {
            return new TheatreResponse
            {
                TheatreId = theatre.TheatreId,
                Name = theatre.Name,
                Location = theatre.Location
            };
        }
    }
}