namespace Varastokkr.IdentityAPI.Endpoints;

internal class RegisterEnpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("account/register",
                async (RegisterDto model,
                ILogger<RegisterEnpoint> logger,
                UserManager<IdentityUser> userManager,
                IValidator<RegisterDto> validator) =>
                {
                    logger.LogInformation("User {@Username} register attempt", model.Email);

                    var validationResult = await validator.ValidateAsync(model);
                    if (!validationResult.IsValid)
                    {
                        logger.LogInformation("User {@Username} register attempt failed", model.Email);
                        return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
                    }

                    var user = await userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        logger.LogInformation("User {@Username} login attempt failed", model.Email);
                        return Results.Problem("User with this email already exists!", statusCode: 401);
                    }

                    user = new IdentityUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.Name,
                        Email = model.Email,
                    };

                    var result = await userManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        logger.LogInformation("User {@Username} register attempt failed", model.Email);
                        return Results.Problem(string.Join(",", result.Errors.Select(x => x.Description)), statusCode: 401);
                    }

                    logger.LogInformation("User {@Username} register attempt succeeded", model.Email);

                    return Results.Ok(new { message = "Registration successful!" });
                })
            .Accepts<RegisterDto>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("UserRegister")
            .WithOpenApi(operation =>
            {
                operation.Summary = "User registration endpoint";
                operation.Description = "Creates a new user account and sends an email verification link.";
                return operation;
            });
    }
}
