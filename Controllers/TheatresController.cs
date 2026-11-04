using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class TheatresController : ControllerBase
    {
        private readonly ITheatreService _theatreService;

        public TheatresController(ITheatreService theatreService)
        {
            _theatreService = theatreService;
        }

        // GET: api/theatres
        [HttpGet]
        public async Task<IActionResult> GetAllTheatres(
            int pageNumber = 1, int pageSize = 10)
        {
            var theatres = await _theatreService.GetAllTheatresAsync(
                pageNumber, pageSize);

            return Ok(theatres);
        }

        // GET: api/theatres/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTheatreById(int id)
        {
            var theatre = await _theatreService.GetTheatreByIdAsync(id);

            if (theatre == null)
            {
                return NotFound(new
                {
                    message = "Theatre not found."
                });
            }

            return Ok(theatre);
        }

        // POST: api/theatres
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> CreateTheatre(
            CreateTheatreRequest request)
        {
            var theatre = await _theatreService.CreateTheatreAsync(request);

            return CreatedAtAction(
                nameof(GetTheatreById),
                new { id = theatre.TheatreId },
                theatre);
        }

        // PUT: api/theatres/1
        [Authorize(Roles = "Admin")]

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTheatre(
            int id, UpdateTheatreRequest request)
        {
            var theatre = await _theatreService.UpdateTheatreAsync(
                id, request);

            if (theatre == null)
            {
                return NotFound(new
                {
                    message = "Theatre not found."
                });
            }

            return Ok(theatre);
        }

        // DELETE: api/theatres/1
        [Authorize(Roles = "Admin")]

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTheatre(int id)
        {
            var result = await _theatreService.DeleteTheatreAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Theatre not found."
                });
            }

            return NoContent();
        }
    }
}