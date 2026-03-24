using Libros;
using Libros.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Libros.Mappings;

var builder = WebApplication.CreateBuilder(args);


// Services
builder.Services.AddScoped<ILibroServices, LibroServices>(); // Inyección de dependencias para el servicio de libros

// AutoMapper
builder.Services.AddAutoMapper(cfg => {}, typeof(MappingProfile));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Recreate database in Development if model changed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware
app.UseCors("AllowFrontend");
app.UseSwagger();
app.UseSwaggerUI();

// Descomenta esta línea si necesitas HTTPS en producción
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
