using Microsoft.AspNetCore.Identity;
using Varastokkr.Shared;

namespace Varastokkr.IdentityAPI.Infrastructure;

public class UsersSeed(ILogger<UsersSeed> logger, UserManager<IdentityUser> userManager) : IDbSeeder<IdentityApiDbContext>
{
    public async Task SeedAsync(IdentityApiDbContext context)
    {
        var alice = await userManager.FindByNameAsync("alice");

        if (alice == null)
        {
            alice = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
            };

            var result = userManager.CreateAsync(alice, "Pass123$").Result;

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("alice created");
        }
        else
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("alice already exists");
        }

        var bob = await userManager.FindByNameAsync("bob");

        if (bob == null)
        {
            bob = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
            };

            var result = await userManager.CreateAsync(bob, "Pass123$");

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("bob created");
        }
        else
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("bob already exists");
        }
    }
}