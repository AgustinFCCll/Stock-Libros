using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Libros.Services;
using Libros.Models;
using Libros.DTOs;
using Libros.Mappings;

namespace Libros.Tests;

public class LibroServicesTests : IDisposable
{
    private readonly AppDbContext context;
    private readonly IMapper mapper;
    private readonly LibroServices service;

    public LibroServicesTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new AppDbContext(options);

        var profile = new MappingProfile();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        var serviceProvider = services.BuildServiceProvider();
        mapper = serviceProvider.GetRequiredService<IMapper>();

        service = new LibroServices(context, mapper);

        SeedData();
    }

    private void SeedData()
    {
        var libros = new List<Libro>
        {
            new() { Id = 1, Titulo = "Libro Prestado Leido", Prestado = true, Leido = true },
            new() { Id = 2, Titulo = "Libro Prestado No Leido", Prestado = true, Leido = false },
            new() { Id = 3, Titulo = "Libro Disponible Leido", Prestado = false, Leido = true },
            new() { Id = 4, Titulo = "Libro Disponible No Leido", Prestado = false, Leido = false }
        };

        context.Libros.AddRange(libros);
        context.SaveChanges();
    }

    public void Dispose()
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }

    [Fact]
    public async Task GetAll_ReturnsAllLibros()
    {
        var result = await service.GetAll();

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public async Task GetByFilter_PrestadoTrue_ReturnsOnlyPrestado()
    {
        var result = await service.GetByFilter(prestado: true, leido: null);

        Assert.Equal(2, result.Count);
        Assert.All(result, l => Assert.True(l.Prestado));
    }

    [Fact]
    public async Task GetByFilter_PrestadoFalse_ReturnsOnlyDisponible()
    {
        var result = await service.GetByFilter(prestado: false, leido: null);

        Assert.Equal(2, result.Count);
        Assert.All(result, l => Assert.False(l.Prestado));
    }

    [Fact]
    public async Task GetByFilter_LeidoTrue_ReturnsOnlyLeido()
    {
        var result = await service.GetByFilter(prestado: null, leido: true);

        Assert.Equal(2, result.Count);
        Assert.All(result, l => Assert.True(l.Leido));
    }

    [Fact]
    public async Task GetByFilter_LeidoFalse_ReturnsOnlyNoLeido()
    {
        var result = await service.GetByFilter(prestado: null, leido: false);

        Assert.Equal(2, result.Count);
        Assert.All(result, l => Assert.False(l.Leido));
    }

    [Fact]
    public async Task GetByFilter_PrestadoAndLeido_ReturnsBoth()
    {
        var result = await service.GetByFilter(prestado: false, leido: true);

        Assert.Single(result);
        Assert.Equal("Libro Disponible Leido", result[0].Titulo);
        Assert.False(result[0].Prestado);
        Assert.True(result[0].Leido);
    }

    [Fact]
    public async Task GetByFilter_NoFilter_ReturnsAll()
    {
        var result = await service.GetByFilter(prestado: null, leido: null);

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public async Task TogglePrestado_TogglesEstado()
    {
        var libro = await context.Libros.FindAsync(4);
        Assert.False(libro!.Prestado);

        var result = await service.TogglePrestado(4);
        Assert.True(result);

        libro = await context.Libros.FindAsync(4);
        Assert.True(libro!.Prestado);
    }

    [Fact]
    public async Task TogglePrestado_NotFound_ReturnsFalse()
    {
        var result = await service.TogglePrestado(999);
        Assert.False(result);
    }

    [Fact]
    public async Task ToggleLeido_TogglesEstado()
    {
        var libro = await context.Libros.FindAsync(4);
        Assert.False(libro!.Leido);

        var result = await service.ToggleLeido(4);
        Assert.True(result);

        libro = await context.Libros.FindAsync(4);
        Assert.True(libro!.Leido);
    }

    [Fact]
    public async Task ToggleLeido_NotFound_ReturnsFalse()
    {
        var result = await service.ToggleLeido(999);
        Assert.False(result);
    }
}
