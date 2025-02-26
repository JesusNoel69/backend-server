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
// Add services to the container.
builder.Services.AddCors(options =>
{
   options.AddPolicy(name:"AllowAngular", policy =>
   {
       policy.WithOrigins("http://localhost:4200")
             .AllowAnyHeader()
             .AllowAnyMethod();
   });
});

builder.Services.AddDbContext<AplicationDbContext>(options=>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esto preserva los nombres de las propiedades (por ejemplo, "Id" en lugar de "id")
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
