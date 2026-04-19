using Microsoft.EntityFrameworkCore;
using ClinicksApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión del appsettings.json
var connectionString = builder.Configuration.GetConnectionString("ClinicksDataBase");

// Registrar el DbContext para que use PostgreSQL
builder.Services.AddDbContext<ClinicksDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configurar CORS para que React pueda conectarse
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173") // El puerto de tu Vite
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
