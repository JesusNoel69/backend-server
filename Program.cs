using BackEnd_Server.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");//builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("DB_CONNECTION_STRING no está configurada en el archivo .env");
}
string url = Environment.GetEnvironmentVariable("URL")??"AllowAll";
// Add services to the container.
builder.Services.AddCors(options =>
{
   options.AddPolicy(name:"AllowAngular", policy =>
   {
       policy.WithOrigins(url)
             .AllowAnyHeader()
             .AllowAnyMethod();
   });
});

builder.Services.AddDbContext<AplicationDbContext>(options=>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
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
app.UseCors("AllowAngular");

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
