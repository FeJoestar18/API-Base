using ApiBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiBase.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserAddress> UserAddresses => Set<UserAddress>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações para User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();

            entity.Property(u => u.Cpf).IsRequired().HasMaxLength(11);
            entity.HasIndex(u => u.Cpf).IsUnique();

            entity.Property(u => u.Name).IsRequired().HasMaxLength(45);

            entity.HasMany(u => u.Addresses)
                  .WithOne(a => a.User)
                  .HasForeignKey(a => a.UserId);
        });

        // Configurações para UserAddress
        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Cep).IsRequired().HasMaxLength(8);
            entity.Property(a => a.Street).IsRequired().HasMaxLength(200);
            entity.Property(a => a.Number).IsRequired().HasMaxLength(20);
            entity.Property(a => a.Complement).HasMaxLength(100);
            entity.Property(a => a.Neighborhood).IsRequired().HasMaxLength(100);
            entity.Property(a => a.City).IsRequired().HasMaxLength(100);
            entity.Property(a => a.State).IsRequired().HasMaxLength(50);
        });
    }
}