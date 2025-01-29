using FluentValidation;
using Varastokkr.Shared.Abstract;

namespace Varastokkr.IdentityAPI.Endpoints;

internal class TestEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("echo", () =>
                {
                    
                    return Results.Ok("ECHO");
                })
            .Produces(StatusCodes.Status200OK)
            .WithName("Echo")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Echo endpoint";
                operation.Description = "Echo";
                return operation;
            });
    }
}
