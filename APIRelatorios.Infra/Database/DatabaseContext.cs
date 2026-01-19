using APIRelatorios.Dommain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIRelatorios.Infra.Database;

public class DatabaseContext : DbContext 
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Imagem> Imagem { get; set; }

    public DbSet<Rota> Rota { get; set; }

    public DbSet<User> Fiscais { get; set; }

    public DbSet<UsuarioRota> UsuarioRotas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Imagem>()
            .HasKey(x => x.ImagemId);

        modelBuilder.Entity<Imagem>()
            .HasOne(rota => rota.Rota)
            .WithMany()
            .HasForeignKey(r => r.RotaID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasKey(x => x.UserId);

        modelBuilder.Entity<Rota>()
            .HasKey(x => x.RotaId);

        modelBuilder.Entity<UsuarioRota>()
            .HasKey(ur => new { ur.UserID, ur.RotaID });

        modelBuilder.Entity<UsuarioRota>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.usuarioRotas)
            .HasForeignKey(ur => ur.UserID);

        modelBuilder.Entity<UsuarioRota>()
            .HasOne(ur => ur.Rota)
            .WithMany(r => r.Fiscais)
            .HasForeignKey(ur => ur.RotaID);


    }
}
