using Microsoft.EntityFrameworkCore;
using Libros.Models;


namespace Libros
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Libro> Libros {  get; set; }
       
    }
}