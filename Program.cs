using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MiProyecto8;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddTransient<ISaludoService, SaludoService>();
//builder.Services.AddSingleton<IContadorService, ContadorService>();

builder.Services.AddScoped<IContadorService, ContadorService>();
//builder.Services.AddSingleton<IContadorService, ContadorService>();
//builder.Services.AddKeyedTransient<ISaludoService, SaludoFormal>("formal");
//builder.Services.AddKeyedTransient<ISaludoService, SaludoInformal>("informal");
builder.Services.AddTransient<ILlamada, Llamadas>();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("jsonplaceholder", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApis:BaseUrl"]);
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

// --- [INICIO DE CÓDIGO A INSERTAR EN Program.cs ANTES DE app.Use... ] ---

// 1. Configurar Autenticación
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// 2. Configurar Autorización (Siempre va después de AddAuthentication)
builder.Services.AddAuthorization();

// 3. Configurar Swagger para JWT (opcional, pero muy recomendable)
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Encabezado de Autorización JWT usando el esquema Bearer. Ejemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- [INICIO DE CÓDIGO A INSERTAR EN Program.cs DESPUÉS DE UseHttpsRedirection ] ---
app.UseAuthentication();
app.UseAuthorization();
// --- [FIN DE CÓDIGO A INSERTAR EN Program.cs DESPUÉS DE UseHttpsRedirection ] ---

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.RequireAuthorization()
.WithOpenApi();
app.MapControllers();
app.Run();


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
