
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // Get all movies
        public async Task<List<MovieResponse>> GetAllMoviesAsync(
            int pageNumber,
            int pageSize,
            string? title)
        {
            var movies = await _movieRepository.GetAllAsync(
                pageNumber, pageSize, title);

            return movies.Select(m => MapToResponse(m)).ToList();
        }

        // Get movie by ID
        public async Task<MovieResponse?> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                return null;
            }

            return MapToResponse(movie);
        }

        // Create a new movie
        public async Task<MovieResponse> CreateMovieAsync(
            CreateMovieRequest request)
        {
            var movie = new Movie
            {
                Title = request.Title,
                Description = request.Description,
                DurationMinutes = request.DurationMinutes,
                ReleaseDate = request.ReleaseDate,
                Language = request.Language,
                Genre = request.Genre
            };

            await _movieRepository.AddAsync(movie);

            await _movieRepository.SaveChangesAsync();

            return MapToResponse(movie);
        }

        // Update an existing movie
        public async Task<MovieResponse?> UpdateMovieAsync(
            int id,
            UpdateMovieRequest request)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                return null;
            }

            movie.Title = request.Title;
            movie.Description = request.Description;
            movie.DurationMinutes = request.DurationMinutes;
            movie.ReleaseDate = request.ReleaseDate;
            movie.Language = request.Language;
            movie.Genre = request.Genre;

            await _movieRepository.SaveChangesAsync();

            return MapToResponse(movie);
        }

        // Delete a movie
        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                return false;
            }

            await _movieRepository.DeleteAsync(movie);

            await _movieRepository.SaveChangesAsync();

            return true;
        }

        // Search movies by title
        public async Task<List<MovieResponse>> SearchMoviesAsync(string title)
        {
            var movies = await _movieRepository.SearchByTitleAsync(title);

            return movies.Select(m => MapToResponse(m)).ToList();
        }

        // Convert Movie entity into MovieResponse DTO
        private static MovieResponse MapToResponse(Movie movie)
        {
            return new MovieResponse
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                Language = movie.Language,
                Genre = movie.Genre
            };
        }
        public async Task<MovieResponse?> PatchMovieAsync(
    int id, PatchMovieRequest request)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                return null;
            }

            if (request.Title != null)
                movie.Title = request.Title;

            if (request.Description != null)
                movie.Description = request.Description;

            if (request.DurationMinutes.HasValue)
                movie.DurationMinutes = request.DurationMinutes.Value;

            if (request.ReleaseDate.HasValue)
                movie.ReleaseDate = request.ReleaseDate.Value;

            if (request.Language != null)
                movie.Language = request.Language;

            if (request.Genre != null)
                movie.Genre = request.Genre;

            await _movieRepository.SaveChangesAsync();

            return MapToResponse(movie);
        }
    }
}
