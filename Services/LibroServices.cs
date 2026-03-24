using AutoMapper;
using Libros.Models;
using Microsoft.EntityFrameworkCore;
using Libros.Services;
using Libros.DTOs;

namespace Libros.Services
{
    public class LibroServices : ILibroServices
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public LibroServices(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<LibroDTO>> GetAll()
        {
            var libros = await context.Libros.ToListAsync();
            return mapper.Map<List<LibroDTO>>(libros);
        }

        public async Task<LibroDTO?> Get(int id)
        {
            var libro = await context.Libros.FindAsync(id);
            return libro == null ? null : mapper.Map<LibroDTO>(libro);
        }

        public async Task Create(CreateLibroDTO createDto)
        {
            var libro = mapper.Map<Libro>(createDto);
            await context.Libros.AddAsync(libro);
            await context.SaveChangesAsync();
        }

        public async Task Update(UpdateLibroDTO updateDto)
        {
            var libro = await context.Libros.FindAsync(updateDto.Id);
            if (libro != null)
            {
                mapper.Map(updateDto, libro);
                context.Libros.Update(libro);
                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var libro = await context.Libros.FindAsync(id);
            if (libro != null)
            {
                context.Libros.Remove(libro);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> TogglePrestado(int id)
        {
            var libro = await context.Libros.FindAsync(id);
            if (libro == null) return false;
            
            libro.Prestado = !libro.Prestado;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<LibroDTO>> GetByFilter(bool? prestado, bool? leido)
        {
            var query = context.Libros.AsQueryable();
            
            if (prestado.HasValue)
                query = query.Where(l => l.Prestado == prestado.Value);
            
            if (leido.HasValue)
                query = query.Where(l => l.Leido == leido.Value);
            
            var libros = await query.ToListAsync();
            return mapper.Map<List<LibroDTO>>(libros);
        }

        public async Task<bool> ToggleLeido(int id)
        {
            var libro = await context.Libros.FindAsync(id);
            if (libro == null) return false;
            
            libro.Leido = !libro.Leido;
            await context.SaveChangesAsync();
            return true;
        }
    }
}