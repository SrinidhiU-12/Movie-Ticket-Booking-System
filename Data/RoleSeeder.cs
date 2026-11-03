using Microsoft.AspNetCore.Identity;
using MovieTicketBooking.Constants;

namespace MovieTicketBooking.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles =
            {
                UserRoles.Admin,
                UserRoles.Customer,
                UserRoles.TheatreManager
            };

            foreach (var role in roles)
            {
                bool roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    var result = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(
                            $"Failed to create role {role}: " +
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }
    }
}