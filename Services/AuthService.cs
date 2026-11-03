using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBooking.DTOs.Auth;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieTicketBooking.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // Register a new customer
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
                throw new InvalidOperationException("Email is already registered.");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }

            // Ensure Customer role exists
            if (!await _roleManager.RoleExistsAsync("Customer"))
            {
                var role = await _roleManager.CreateAsync(
                    new IdentityRole("Customer"));

                if (!role.Succeeded)
                    throw new InvalidOperationException("Could not create Customer role.");
            }

            // Assign Customer role
            await _userManager.AddToRoleAsync(user, "Customer");

            return await GenerateTokenAsync(user, "Customer");
        }

        // Login an existing customer
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(
                user, request.Password);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count == 0)
                throw new UnauthorizedAccessException("User has no assigned role.");

            return await GenerateTokenAsync(user, roles[0]);
        }

        // Generate JWT token
        private async Task<AuthResponse> GenerateTokenAsync(
            ApplicationUser user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, role)
            };

            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("JWT key is not configured.");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddHours(2);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new AuthResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt,
                Role = role
            };
        }
    }
}