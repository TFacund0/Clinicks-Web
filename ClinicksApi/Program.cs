using ClinicksApi.Business;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.Services;
using ClinicksApi.Data;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión del appsettings.json
var connectionString = builder.Configuration.GetConnectionString("ClinicksDataBase");

// Registrar el DbContext para que use PostgreSQL
builder.Services.AddDbContext<ClinicksDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configurar CORS para que React pueda conectarse
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => {
        policy.WithOrigins("http://localhost:5173") // El puerto de tu React
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configurar Autenticación con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Add services to the container.

// CONEXIONES DE LAS INTERFACES

// Capa de Negocio (Servicios)
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

// Capa de Datos
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ClinicksApi.Middlewares.ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
