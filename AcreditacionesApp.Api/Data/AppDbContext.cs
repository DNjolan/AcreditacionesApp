using AcreditacionesApp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcreditacionesApp.Api.Data
{
    public class AppDbContext: DbContext
    {
        // Recibe las opciones (cadena de conexi[on, proveedor) y se las pasa al padre con ": base(options)".
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Un DbSet por tabla. "=> Set<T>" evita warnings de nulos.
        public DbSet<TipoAcreditacion> TiposAcreditacion => Set<TipoAcreditacion>();
        public DbSet<Acreditacion> Acreditaciones => Set<Acreditacion>();
        public DbSet<RequisitoAcreditacion> Requisitos => Set<RequisitoAcreditacion>();

        // Aqu[i se afina el mapeo entidad <-> tabla.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoAcreditacion>(e =>
            {
                e.ToTable("tipo_acreditacion");
                e.Property(t => t.Nombre).HasMaxLength(100).IsRequired();
                e.Property(t => t.Descripcion).HasMaxLength(300);
                e.HasIndex(t => t.Nombre).IsUnique();
            });

            modelBuilder.Entity<Acreditacion>(e =>
            {
                e.ToTable("acreditacion");
                e.Property(a => a.Nombre).HasMaxLength(150).IsRequired();
                e.Property(a => a.EntidadAcreditadora).HasMaxLength(150).IsRequired();
                e.Property(a => a.Estado).HasConversion<string>().HasMaxLength(20);

                e.HasMany(a => a.Requisitos)
                    .WithOne()
                    .HasForeignKey(r => r.AcreditacionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RequisitoAcreditacion>(e =>
            {
                e.ToTable("requisitos_acreditacion");
                e.Property(r => r.Descripcion).HasMaxLength(300).IsRequired();
            });
        }
    }
}
