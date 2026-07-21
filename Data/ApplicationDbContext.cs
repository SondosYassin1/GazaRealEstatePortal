using Microsoft.EntityFrameworkCore;
using GazaRealEstatePortal.Models;
using GazaRealEstatePortal.Models.Enums;
using System;

namespace GazaRealEstatePortal.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Property> Properties { get; set; } = null!;
    public DbSet<PropertyImage> PropertyImages { get; set; } = null!;
    public DbSet<PropertyReview> PropertyReviews { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Unique Index on User.Email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // 2. Convert Enums to string in DB
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .Property(p => p.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .Property(p => p.OperationType)
            .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .Property(p => p.PropertyType)
            .HasConversion<string>();

        modelBuilder.Entity<PropertyReview>()
            .Property(pr => pr.Decision)
            .HasConversion<string>();

        // 3. User -> Property (1 to N)
        modelBuilder.Entity<Property>()
            .HasOne(p => p.User)
            .WithMany(u => u.Properties)
            .HasForeignKey(p => p.UserId);

        // 4. Property -> PropertyImage (1 to N) with Cascade Delete
        modelBuilder.Entity<PropertyImage>()
            .HasOne(pi => pi.Property)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        // 5. Property -> PropertyReview (1 to 1)
        modelBuilder.Entity<PropertyReview>()
            .HasOne(pr => pr.Property)
            .WithOne(p => p.Review)
            .HasForeignKey<PropertyReview>(pr => pr.PropertyId);

        // 6. User (Admin) -> PropertyReview (1 to N) with Restrict Delete
        modelBuilder.Entity<PropertyReview>()
            .HasOne(pr => pr.Admin)
            .WithMany(u => u.ReviewsMade)
            .HasForeignKey(pr => pr.AdminId)
            .OnDelete(DeleteBehavior.Restrict);

        // 7. Seed fixed Admin account
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            FullName = "System Admin",
            Email = "admin@gaza-realestate.com",
            PasswordHash = "$2a$11$Opgsk4F5QPn18MUikFvbf.phbT36mFqvL2t4mIko1P/lkeQu7xMrm",
            PhoneNumber = "0590000000",
            Role = UserRole.Admin,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            IsActive = true
        });
    }
}
