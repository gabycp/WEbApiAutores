using Microsoft.EntityFrameworkCore;
using WEbApiAutores.Controllers.Entidades;

namespace WEbApiAutores
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libros> Libros { get; set; }
    }
}
