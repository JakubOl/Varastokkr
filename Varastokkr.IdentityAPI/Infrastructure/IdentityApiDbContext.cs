using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Varastokkr.IdentityAPI.Infrastructure;

public class IdentityApiDbContext(DbContextOptions<IdentityApiDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder) => base.OnModelCreating(builder);
}