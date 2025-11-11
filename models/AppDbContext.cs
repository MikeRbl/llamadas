using Microsoft.EntityFrameworkCore;
namespace MiProyecto8;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Convocatoria> Convocatorias { get; set; }
    public DbSet<AlumnoM> Alumnos { get; set; }

        public DbSet<Participantes> Participantes { get; set; }

}
