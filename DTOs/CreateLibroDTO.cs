namespace Libros.DTOs
{
    public class CreateLibroDTO
    {
        public string? Titulo { get; set; }
        public string? Imagen { get; set; }
        public bool Prestado { get; set; } = false;
        public bool Leido { get; set; } = false;
    }
}
