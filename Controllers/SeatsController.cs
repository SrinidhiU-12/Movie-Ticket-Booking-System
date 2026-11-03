
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        // GET: api/seats/screen/1
        [HttpGet("screen/{screenId:int}")]
        public async Task<IActionResult> GetSeatsByScreenId(int screenId)
        {
            var seats = await _seatService.GetSeatsByScreenIdAsync(screenId);
            return Ok(seats);
        }

        // GET: api/seats/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSeatById(int id)
        {
            var seat = await _seatService.GetSeatByIdAsync(id);

            if (seat == null)
                return NotFound(new { message = "Seat not found." });

            return Ok(seat);
        }

        // POST: api/seats
        [HttpPost]
        public async Task<IActionResult> CreateSeat(CreateSeatRequest request)
        {
            var seat = await _seatService.CreateSeatAsync(request);

            return CreatedAtAction(
                nameof(GetSeatById),
                new { id = seat.SeatId },
                seat);
        }

        // POST: api/seats/bulk
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreateSeats(BulkCreateSeatsRequest request)
        {
            var seats = await _seatService.BulkCreateSeatsAsync(request);

            return StatusCode(StatusCodes.Status201Created, seats);
        }

        // PUT: api/seats/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSeat(
            int id,
            UpdateSeatRequest request)
        {
            var seat = await _seatService.UpdateSeatAsync(id, request);

            if (seat == null)
                return NotFound(new { message = "Seat not found." });

            return Ok(seat);
        }

        // DELETE: api/seats/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var result = await _seatService.DeleteSeatAsync(id);

            if (!result)
                return NotFound(new { message = "Seat not found." });

            return NoContent();
        }
    }
}
