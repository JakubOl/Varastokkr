using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Varastokkr.IdentityAPI.Infrastructure;
using Varastokkr.Shared;
using Varastokkr.Shared.Extensions;

var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

var dbConnectionString = builder.Configuration.GetConnectionString("IdentityDbConnectionString");
builder.Services.AddDbContext<IdentityApiDbContext>(options => options.UseSqlServer(dbConnectionString));
//builder.Services.AddMigration<IdentityApiDbContext, UsersSeed>();

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityApiDbContext>()
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
