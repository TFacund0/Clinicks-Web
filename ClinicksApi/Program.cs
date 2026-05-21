using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.Services;
using ClinicksApi.Data;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// ====================================================================
// FASE 1: CONSTRUCCIÓN DEL SERVIDOR (BUILDER)
// Aquí configuramos todas las "herramientas" que nuestro servidor 
// necesitará antes de arrancar (Base de datos, CORS, JWT, etc.)
// ====================================================================
var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión del appsettings.json
var connectionString = builder.Configuration.GetConnectionString("ClinicksDataBase");
builder.Services.AddDbContext<ClinicksDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configurar CORS para que React pueda conectarse usando configuración
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:5173" };

builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => {
        policy.WithOrigins(allowedOrigins)
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

// ====================================================================
// INYECCIÓN DE DEPENDENCIAS (LA FÁBRICA DE OBJETOS)
// ====================================================================

// Capa de Datos (Repositorios)
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<IProcesoRepository, ProcesoRepository>();

// Capa de Negocio (Servicios)
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddScoped<IProcesoService, ProcesoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ====================================================================
// FASE 2: ENSAMBLAJE Y EJECUCIÓN DE LA APLICACIÓN (APP)
// ====================================================================
var app = builder.Build();

// 1. MIDDLEWARE GLOBAL DE EXCEPCIONES: Atrapa cualquier error fatal
app.UseMiddleware<ClinicksApi.Middlewares.ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ====================================================================
// FASE 3: INICIALIZACIÓN DE DATOS MAESTROS (SEEDING)
// ====================================================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClinicksDbContext>();
    await DbInitializer.SeedAsync(context);
}

app.Run();