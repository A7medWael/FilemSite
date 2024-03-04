using FilemSite.Models;
using Microsoft.EntityFrameworkCore;

namespace FilemSite.ContextModel
{
    public class FilmContext:DbContext
    {
        public FilmContext(DbContextOptions<FilmContext> dbContext):base(dbContext)
        {
            
        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
