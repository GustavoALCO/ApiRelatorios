using APIRelatorios.Domain.Entities;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Enuns;
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

    public DbSet<CheckList> CheckList {get; set;}

    public DbSet<Amostra> Amostras { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // EvidenciaRota

    modelBuilder.Entity<EvidenciaRota>()
        .HasKey(x => x.EvidenciaRotaId);

    modelBuilder.Entity<EvidenciaRota>()
        .HasOne(e => e.Rota)
        .WithMany(r => r.Images)
        .HasForeignKey(e => e.RotaId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<EvidenciaRota>()
        .HasOne(e => e.CheckList)
        .WithOne(c => c.EvidenciaRota)
        .HasForeignKey<CheckList>(c => c.EvidenciaRotaId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<EvidenciaRota>()
        .HasMany(e => e.Images)
        .WithOne(i => i.EvidenciaRota)
        .HasForeignKey(i => i.EvidenciaRotaId)
        .OnDelete(DeleteBehavior.Cascade);

    // CheckList

    modelBuilder.Entity<CheckList>()
        .HasKey(c => c.Id);

    modelBuilder.Entity<CheckList>()
        .Property(c => c.TemaCheck)
        .HasConversion<int>();

    modelBuilder.Entity<CheckList>()
        .Property(c => c.SubTemaAlimentadores)
        .HasConversion(
            v => v.Select(e => (int)e).ToArray(),
            v => v.Select(e => (SubTemaAlimentadores)e).ToList());

    // User

    modelBuilder.Entity<User>()
        .HasKey(x => x.UserId);

    // Rota

    modelBuilder.Entity<Rota>()
        .HasKey(x => x.RotaId);

    modelBuilder.Entity<Rota>()
        .HasDiscriminator<string>("TipoRota")
        .HasValue<Rota>("Rota")
        .HasValue<RotaRetorno>("RotaRetorno");

    // UsuarioRota

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

    // Amostra

    modelBuilder.Entity<Amostra>()
                .HasKey(x => x.Id);

    modelBuilder.Entity<Amostra>()
                .HasOne(e => e.Rota)
                .WithMany()
                .HasForeignKey(e => e.RotaId);

        base.OnModelCreating(modelBuilder);
}
}
