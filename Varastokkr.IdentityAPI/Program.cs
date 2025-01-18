using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Varastokkr.IdentityAPI.Infrastructure;
using Varastokkr.Shared;
using Varastokkr.Shared.Extensions;

var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(dbConnectionString));
builder.Services.AddMigration<IdentityDbContext, UsersSeed>();

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<TokenGenerator, TokenGenerator>();

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
