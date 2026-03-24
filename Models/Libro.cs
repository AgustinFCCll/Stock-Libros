namespace Libros.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Imagen {  get; set; }
        public bool Prestado { get; set; } = false;
        public bool Leido { get; set; } = false;

    }
}
