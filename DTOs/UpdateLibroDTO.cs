namespace Libros.DTOs
{
    public class UpdateLibroDTO
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Imagen { get; set; }
        public bool Prestado { get; set; }
        public bool Leido { get; set; }
    }
}
