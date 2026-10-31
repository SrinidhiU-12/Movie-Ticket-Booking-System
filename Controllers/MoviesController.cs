
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Controllers;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // 1. Get all movies with pagination and optional title filter
        [HttpGet]
        public async Task<IActionResult> GetAllMovies(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? title = null)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            var movies = await _movieService.GetAllMoviesAsync(
                pageNumber, pageSize, title);

            return Ok(movies);
        }

        // 2. Get a movie by ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound("Movie not found.");
            }

            return Ok(movie);
        }

        // 3. Create a new movie
        [HttpPost]
        public async Task<IActionResult> CreateMovie(
            [FromBody] CreateMovieRequest request)
        {
            var movie = await _movieService.CreateMovieAsync(request);

            return CreatedAtAction(
                nameof(GetMovieById),
                new { id = movie.MovieId },
                movie);
        }

        // 4. Update an existing movie
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMovie(
            int id,
            [FromBody] UpdateMovieRequest request)
        {
            var movie = await _movieService.UpdateMovieAsync(id, request);

            if (movie == null)
            {
                return NotFound("Movie not found.");
            }

            return Ok(movie);
        }

        // 5. Delete a movie
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var isDeleted = await _movieService.DeleteMovieAsync(id);

            if (!isDeleted)
            {
                return NotFound("Movie not found.");
            }

            return NoContent();
        }

        // 6. Search movies by title
        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies(
            [FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Search title is required.");
            }

            var movies = await _movieService.SearchMoviesAsync(title);

            return Ok(movies);
        }
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchMovie(
    int id, PatchMovieRequest request)
        {
            var movie = await _movieService.PatchMovieAsync(id, request);

            if (movie == null)
            {
                return NotFound(new
                {
                    message = "Movie not found."
                });
            }

            return Ok(movie);
        }
    }
}
