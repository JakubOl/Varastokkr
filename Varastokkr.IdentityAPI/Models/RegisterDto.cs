namespace Varastokkr.IdentityAPI.Models;

public record RegisterDto
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
}
