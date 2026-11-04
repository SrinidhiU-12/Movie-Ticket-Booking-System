using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
namespace MovieTicketBooking.Controllers
{
    [Route("api/shows/{showId:int}/seats")]
    [ApiController]
    [Authorize]

    public class ShowSeatsController : ControllerBase
    {
        private readonly IShowSeatService _showSeatService;

        public ShowSeatsController(IShowSeatService showSeatService)
        {
            _showSeatService = showSeatService;
        }

        // GET: api/shows/1/seats
        [HttpGet]
        public async Task<IActionResult> GetAllSeats(int showId)
        {
            var seats = await _showSeatService.GetAllSeatsAsync(showId);

            if (seats == null)
                return NotFound("Show not found.");

            return Ok(seats);
        }

        // GET: api/shows/1/seats/available
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableSeats(int showId)
        {
            var seats = await _showSeatService.GetAvailableSeatsAsync(showId);

            if (seats == null)
                return NotFound("Show not found.");

            return Ok(seats);
        }

        // GET: api/shows/1/seats/booked
        [HttpGet("booked")]
        public async Task<IActionResult> GetBookedSeats(int showId)
        {
            var seats = await _showSeatService.GetBookedSeatsAsync(showId);

            if (seats == null)
                return NotFound("Show not found.");

            return Ok(seats);
        }
    }
}