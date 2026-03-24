using Libros.Models;
using Libros.DTOs;

namespace Libros.Services
{
    public interface ILibroServices
    {
        Task<List<LibroDTO>> GetAll();
        Task<LibroDTO?> Get(int id);
        Task Create(CreateLibroDTO createDto);
        Task Update(UpdateLibroDTO updateDto);
        Task Delete(int id);
        Task<bool> TogglePrestado(int id);
        Task<List<LibroDTO>> GetByFilter(bool? prestado, bool? leido);
        Task<bool> ToggleLeido(int id);
    }
}