﻿namespace Varastokkr.IdentityAPI.Models;

internal record RegisterDto
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
}
