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


var connectionString = builder.Configuration.GetConnectionString("ClinicksDataBase");
builder.Services.AddDbContext<ClinicksDbContext>(options =>
    options.UseNpgsql(connectionString));


var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:5173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


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


builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<IProcesoRepository, ProcesoRepository>();


builder.Services.AddScoped<ITurnoService, TurnoService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddScoped<IProcesoService, ProcesoService>();


builder.Services.AddHostedService<ServicioLimpiezaTurnos>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
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


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClinicksDbContext>();
    await DbInitializer.SeedAsync(context);
}

app.Run();