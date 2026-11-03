//User Email	Role
//System Admin	admin@moviebooking.com	Admin
//Theatre Manager	manager@moviebooking.com	TheatreManager
using Microsoft.AspNetCore.Identity;
using MovieTicketBooking.Constants;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var accounts = new[]
            {
                new
                {
                    FullName = "System Admin",
                    Email = "admin@moviebooking.com",
                    Role = UserRoles.Admin
                },
                new
                {
                    FullName = "Theatre Manager",
                    Email = "manager@moviebooking.com",
                    Role = UserRoles.TheatreManager
                }
            };

            foreach (var account in accounts)
            {
                var user = await userManager.FindByEmailAsync(
                    account.Email);

                if (user == null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Create account: {account.Email}");
                    Console.Write("Enter password: ");

                    string password = ReadPassword();

                    user = new ApplicationUser
                    {
                        FullName = account.FullName,
                        Email = account.Email,
                        UserName = account.Email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(
                        user, password);

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(
                            string.Join(", ",
                                result.Errors.Select(e => e.Description)));
                    }

                    Console.WriteLine("\nUser created successfully.");
                }
                else
                {
                    Console.WriteLine(
                        $"User already exists: {account.Email}");
                }

                if (!await userManager.IsInRoleAsync(
                    user, account.Role))
                {
                    var roleResult = await userManager.AddToRoleAsync(
                        user, account.Role);

                    if (!roleResult.Succeeded)
                    {
                        throw new InvalidOperationException(
                            string.Join(", ",
                                roleResult.Errors.Select(e => e.Description)));
                    }
                }

                Console.WriteLine(
                    $"Role assigned: {account.Role}");
            }
        }

        private static string ReadPassword()
        {
            var password = new System.Text.StringBuilder();

            while (true)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                    break;

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                        password.Length--;

                    continue;
                }

                if (!char.IsControl(key.KeyChar))
                    password.Append(key.KeyChar);
            }

            return password.ToString();
        }
    }
}
