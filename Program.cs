using BackEnd_Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using System.Text;
using BackEnd_Server.Controllers;
using BackEnd_Server.Services;
//dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0-preview.7.23375.9

var builder = WebApplication.CreateBuilder(args);
Env.Load(); //quitar en produccion
// var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");//builder.Configuration.GetConnectionString("DefaultConnection");
// Console.WriteLine("🔎 DB_CONNECTION_STRING: " + connectionString);
// if (string.IsNullOrEmpty(connectionString))
// {
//     throw new Exception("DB_CONNECTION_STRING no está configurada en el archivo .env");
// }


string url = Environment.GetEnvironmentVariable("URL")??"AllowAll";
var jwtSecret = Environment.GetEnvironmentVariable("JWT")??"";
var key = Encoding.ASCII.GetBytes(jwtSecret);



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // En producción, HTTPS debería estar activado
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // Puedes especificar el emisor (issuer) si lo deseas
        ValidateAudience = false // Lo mismo para la audiencia (audience)
    };
});

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins(["https://lucky-cendol-dda481.netlify.app", "http://localhost:4200"])
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddDbContext<AplicationDbContext>(options=>
    options.UseMySql("server=185.42.105.187;port=3306;database=scrumdb;user=jesusnoel;password=Test.Password", ServerVersion.AutoDetect("server=185.42.105.187;port=3306;database=scrumdb;user=jesusnoel;password=Test.Password")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<SecurityScannerService>();  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngular");

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
