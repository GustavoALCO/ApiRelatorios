using APIRelatorios.Dommain.Entities;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.EntityFrameworkCore;

namespace APIRelatorios.Infra.Database;

public class DatabaseContext : DbContext 
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<EvidenciaRota> EvidenciaRota { get; set; }

    public DbSet<Rota> Rota { get; set; }

    public DbSet<User> Fiscais { get; set; }

    public DbSet<UsuarioRota> UsuarioRotas { get; set; }

    public DbSet<ImageData> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EvidenciaRota>()
            .HasKey(x => x.EvidenciaRotaId);

        modelBuilder.Entity<EvidenciaRota>()
            .HasOne(rota => rota.Rota)
            .WithMany(r => r.Images)
            .HasForeignKey(r => r.RotaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EvidenciaRota>()
                    .HasMany(e => e.Images)
                    .WithOne(i => i.EvidenciaRota)
                    .HasForeignKey(i => i.EvidenciaRotaId)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasKey(x => x.UserId);

        modelBuilder.Entity<Rota>()
            .HasKey(x => x.RotaId);

        modelBuilder.Entity<Rota>()
                    .HasDiscriminator<string>("TipoRota")
                    .HasValue<Rota>("Rota")
                    .HasValue<RotaRetorno>("RotaRetorno");

        modelBuilder.Entity<UsuarioRota>()
            .HasKey(ur => new { ur.UserId, ur.RotaId });

        modelBuilder.Entity<UsuarioRota>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.usuarioRotas)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UsuarioRota>()
            .HasOne(ur => ur.Rota)
            .WithMany(r => r.Fiscais)
            .HasForeignKey(ur => ur.RotaId);


    }
}
