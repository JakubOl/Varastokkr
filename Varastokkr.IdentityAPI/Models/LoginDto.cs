﻿namespace Varastokkr.IdentityAPI.Models;

internal record LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
