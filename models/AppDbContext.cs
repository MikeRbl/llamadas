using Microsoft.EntityFrameworkCore;
namespace MiProyecto8;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Convocatoria> Convocatorias { get; set; }
}
