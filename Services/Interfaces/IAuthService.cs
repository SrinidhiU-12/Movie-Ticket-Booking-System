using MovieTicketBooking.DTOs.Auth;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IAuthService
    {
        // Register a new customer
        Task<AuthResponse> RegisterAsync(RegisterRequest request);

        // Authenticate an existing customer
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}