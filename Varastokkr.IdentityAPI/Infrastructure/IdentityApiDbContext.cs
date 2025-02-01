using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Varastokkr.IdentityAPI.Infrastructure;

internal class IdentityApiDbContext(DbContextOptions<IdentityApiDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder) => base.OnModelCreating(builder);
}