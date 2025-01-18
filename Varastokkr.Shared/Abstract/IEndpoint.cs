using Microsoft.AspNetCore.Routing;

namespace Varastokkr.Shared.Abstract;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
