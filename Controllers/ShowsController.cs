using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        // GET: api/shows
        [HttpGet]
        public async Task<IActionResult> GetAllShows(
            [FromQuery] DateTime? date,
            [FromQuery] int? movieId,
            [FromQuery] int? theatreId)
        {
            var shows = await _showService.GetAllShowsAsync(
                date, movieId, theatreId);

            return Ok(shows);
        }

        // GET: api/shows/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetShowById(int id)
        {
            var show = await _showService.GetShowByIdAsync(id);

            if (show == null)
                return NotFound(new { message = "Show not found." });

            return Ok(show);
        }

        // GET: api/shows/movie/1
        [HttpGet("movie/{movieId:int}")]
        public async Task<IActionResult> GetShowsByMovie(int movieId)
        {
            var shows = await _showService.GetShowsByMovieIdAsync(movieId);
            return Ok(shows);
        }

        // GET: api/shows/theatre/1
        [HttpGet("theatre/{theatreId:int}")]
        public async Task<IActionResult> GetShowsByTheatre(int theatreId)
        {
            var shows = await _showService.GetShowsByTheatreIdAsync(theatreId);
            return Ok(shows);
        }

        // GET: api/shows/search?date=2026-11-05
        [HttpGet("search")]
        public async Task<IActionResult> SearchShows([FromQuery] DateTime date)
        {
            var shows = await _showService.GetAllShowsAsync(
                date, null, null);

            return Ok(shows);
        }

        // POST: api/shows
        [HttpPost]
        public async Task<IActionResult> CreateShow(CreateShowRequest request)
        {
            try
            {
                var show = await _showService.CreateShowAsync(request);

                return CreatedAtAction(
                    nameof(GetShowById),
                    new { id = show.ShowId },
                    show);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/shows/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShow(
            int id, UpdateShowRequest request)
        {
            try
            {
                var show = await _showService.UpdateShowAsync(id, request);

                if (show == null)
                    return NotFound(new { message = "Show not found." });

                return Ok(show);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/shows/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShow(int id)
        {
            var result = await _showService.DeleteShowAsync(id);

            if (!result)
                return NotFound(new { message = "Show not found." });

            return NoContent();
        }
    }
}