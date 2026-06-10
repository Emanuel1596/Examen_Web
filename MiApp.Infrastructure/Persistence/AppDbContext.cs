using Microsoft.EntityFrameworkCore;
using MiApp.Domain.Entities;

namespace MiApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    public DbSet<Event> Events => Set<Event>();

    public DbSet<TicketZone> TicketZones => Set<TicketZone>();

    public DbSet<TicketPurchase> TicketPurchases => Set<TicketPurchase>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Role).HasConversion<string>().IsRequired();
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.Place).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasConversion<string>().IsRequired();

            entity.HasMany(e => e.TicketZones)
                .WithOne(z => z.Event)
                .HasForeignKey(z => z.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Purchases)
                .WithOne(p => p.Event)
                .HasForeignKey(p => p.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TicketZone>(entity =>
        {
            entity.HasKey(z => z.Id);
            entity.Property(z => z.Zone).HasConversion<string>().IsRequired();
            entity.Property(z => z.Price).HasColumnType("decimal(18,2)").IsRequired();

            entity.HasIndex(z => new { z.EventId, z.Zone }).IsUnique();

            entity.HasMany(z => z.Purchases)
                .WithOne(p => p.TicketZone)
                .HasForeignKey(p => p.TicketZoneId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TicketPurchase>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Quantity).IsRequired();
            entity.Property(p => p.Total).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(p => p.PurchaseDate).IsRequired();
        });
    }
}