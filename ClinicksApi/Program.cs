// IMPORTANTE: En el siguiente paso vamos a arreglar los servicios y sus namespaces
// por ahora dejalos apuntando a donde estén (Services o Business)
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.Services;
using ClinicksApi.Data;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. CONEXIÓN A BASE DE DATOS
var connectionString = builder.Configuration.GetConnectionString("ClinicksDataBase");
builder.Services.AddDbContext<ClinicksDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. CORS (Para React)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// 3. INYECCIÓN DE DEPENDENCIAS (DATA LAYER) - ¡Sin duplicados!
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();

// 4. INYECCIÓN DE DEPENDENCIAS (BUSINESS LAYER)
// (Ajustaremos esto cuando pasemos a la capa de Servicios)
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();

// 5. CONFIGURACIÓN DE LA API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

app.Run();