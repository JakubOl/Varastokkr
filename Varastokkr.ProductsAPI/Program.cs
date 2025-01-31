using Asp.Versioning;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Varastokkr.Shared.Extensions;
using Varastokkr.Shared;
using Varastokkr.ProductsAPI.Infrastructure;

var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

var dbConnectionString = builder.Configuration.GetConnectionString("ProductDbConnectionString");
builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(dbConnectionString));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.AddDefaultAuthentication();

var withApiVersioning = builder.Services.AddApiVersioning();
builder.AddDefaultOpenApi(withApiVersioning);

builder.ConfigureOpenTelemetry();
builder.AddDefaultHealthChecks();

builder.Services.AddEndpoints(assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssembly(assembly);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    dbContext.Database.Migrate();
}

app.MapDefaultEndpoints();

var apiVersionSet = app
    .NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

RouteGroupBuilder versionedGroup = app
    .MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

app.MapEndpoints(versionedGroup);
app.UseHttpsRedirection();

app.UseDefaultOpenApi();
app.UseAuthorization();
app.UseAuthentication();

app.Run();