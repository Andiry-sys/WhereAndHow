using Core.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context;

public class UserContext(DbContextOptions<UserContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Address>? Address { get; set; }
    public DbSet<Apartament>? Apartaments { get; set; }
    public override DbSet<User> Users { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public virtual DbSet<Photo>? Photo { get; set; }
    public DbSet<HistoryApartament>? Histories { get; set; }
    public DbSet<PartnerRequest> PartnerRequests { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder
            .Entity<Apartament>()
            .HasOne(a => a.Address)
            .WithMany(ad => ad.Apartaments)
            .HasForeignKey(a => a.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<Apartament>()
            .HasOne(a => a.History)
            .WithOne(h => h.Apartament)
            .HasForeignKey<Apartament>(a => a.HistoryId);
        builder
            .Entity<User>()
            .HasMany(u => u.Apartaments)
            .WithOne(a => a.Owner)
            .HasForeignKey(a => a.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .Entity<PartnerRequest>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore<IdentityUserLogin<string>>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(false);
        base.OnConfiguring(optionsBuilder);
    }
}
