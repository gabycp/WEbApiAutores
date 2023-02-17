using Microsoft.EntityFrameworkCore;
using WEbApiAutores.Entidades;

namespace WEbApiAutores
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AutoresLibros>()
                .HasKey(al => new { al.AutorId, al.LibrosId });
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<AutoresLibros> AutoresLibros { get; set;}
    }
}
