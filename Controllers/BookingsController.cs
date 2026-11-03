using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? showId,
            [FromQuery] string? status)
        {
            var bookings = await _bookingService.GetAllBookingsAsync(
                showId, status);

            return Ok(bookings);
        }

        // GET: api/bookings/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            return Ok(booking);
        }

        // GET: api/bookings/show/1
        [HttpGet("show/{showId:int}")]
        public async Task<IActionResult> GetByShowId(int showId)
        {
            var bookings = await _bookingService.GetBookingsByShowIdAsync(
                showId);

            return Ok(bookings);
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateBookingRequest request)
        {
            try
            {
                var booking = await _bookingService.CreateBookingAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = booking.BookingId },
                    booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/bookings/1/cancel
        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var booking = await _bookingService.CancelBookingAsync(id);

                if (booking == null)
                    return NotFound(new { message = "Booking not found." });

                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}