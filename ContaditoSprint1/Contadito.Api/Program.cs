using System.Text;
using Contadito.Api.Data;
using Contadito.Api.Infrastructure.Security;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using EFCore.NamingConventions; // <-- Importante para UseSnakeCaseNamingConvention

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration);
});

// DbContext (MySQL + snake_case)
builder.Services.AddDbContext<AppDbContext>(opts =>
{
    var cs = builder.Configuration.GetConnectionString("Default");
    opts
        .UseMySql(cs, ServerVersion.AutoDetect(cs))
        .UseSnakeCaseNamingConvention(); // <-- Fuerza snake_case en tablas/columnas/índices
});

// Auth JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtOptions>(jwtSection);
var jwtOptions = jwtSection.Get<JwtOptions>();
if (jwtOptions is null || string.IsNullOrWhiteSpace(jwtOptions.Key))
{
    throw new InvalidOperationException(
        "JWT configuration missing. Please set Jwt:Key (appsettings.json or environment).\n" +
        "Tip: edit Contadito.Api/appsettings.json and replace Jwt:Key with a long random secret."
    );
}
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = key
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (ajusta orígenes para Expo)
builder.Services.AddCors(o => o.AddPolicy("expo", p => p
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(
        "http://localhost:8081",
        "http://localhost:19006",
        "http://127.0.0.1:19006",
        "exp://localhost"
    )));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseCors("expo");

// Endpoint de verificación (sin auth)
app.MapGet("/", () => Results.Ok("Contadito API is running ✅")).AllowAnonymous();

app.UseAuthentication();

// Tenancy middleware: exige tenant_id en JWT para rutas que no sean /auth ni raíz
app.Use(async (ctx, next) =>
{
    if (ctx.Request.Path.StartsWithSegments("/auth") || ctx.Request.Path == "/")
    {
        await next();
        return;
    }

    var tid = ctx.User?.FindFirst("tenant_id")?.Value;
    if (string.IsNullOrEmpty(tid))
    {
        ctx.Response.StatusCode = 401;
        await ctx.Response.WriteAsync("Missing tenant context");
        return;
    }

    ctx.Items["TenantId"] = long.Parse(tid);
    await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
