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

        // 7. Seed fixed Admin account and test user
        var now = new DateTime(2026, 7, 30, 14, 55, 9, DateTimeKind.Utc);
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FullName = "System Admin",
                Email = "admin@gaza-realestate.com",
                PasswordHash = "$2a$11$Opgsk4F5QPn18MUikFvbf.phbT36mFqvL2t4mIko1P/lkeQu7xMrm",
                PhoneNumber = "0590000000",
                Role = UserRole.Admin,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new User
            {
                Id = 2,
                FullName = "Sondos Alaa Yassin",
                Email = "sondosalaa687@gmail.com",
                PasswordHash = "$2a$11$EbXyQBCIEPmReCYun1S4e.E7hs9PNHy1stk841TCweJ4hCK9g37mG",
                PhoneNumber = "0593617699",
                Role = UserRole.RegisteredUser,
                CreatedAt = now,
                IsActive = true
            }
        );

        modelBuilder.Entity<Property>().HasData(
            new Property { Id = 1, UserId = 2, Title = "شقة سكنية فاخرة", Description = "شقة سكنية تشطيب سوبر ديلوكس في الرمال.", Price = 120000m, OperationType = OperationType.Sale, PropertyType = PropertyType.Apartment, Governorate = "غزة", CityAreaCamp = "الرمال", DetailedAddress = "شارع عمر المختار", Area = 150m, Rooms = 3, Bathrooms = 2, Floor = "2", Features = "موقف سيارة, مصعد", ContactPhone = "0593617699", WhatsAppNumber = "00970593617699", Status = PropertyStatus.Approved, CreatedAt = now, UpdatedAt = now },
            new Property { Id = 2, UserId = 2, Title = "فيلا حديثة التصميم", Description = "فيلا حديثة في خانيونس البلد بمساحة ممتازة.", Price = 400m, OperationType = OperationType.Rent, PropertyType = PropertyType.Villa, Governorate = "خانيونس", CityAreaCamp = "البلد", DetailedAddress = "وسط البلد", Area = 300m, Rooms = 5, Bathrooms = 3, Floor = "0", Features = "حديقة, كراج", ContactPhone = "0593617699", WhatsAppNumber = "00970593617699", Status = PropertyStatus.Approved, CreatedAt = now, UpdatedAt = now },
            new Property { Id = 3, UserId = 2, Title = "أرض زراعية", Description = "أرض زراعية خصبة في بيت لاهيا.", Price = 50000m, OperationType = OperationType.Sale, PropertyType = PropertyType.Land, Governorate = "الشمال", CityAreaCamp = "بيت لاهيا", DetailedAddress = "منطقة العطاطرة", Area = 1000m, Rooms = 0, Bathrooms = 0, Floor = "0", Features = "بئر ماء", ContactPhone = "0593617699", WhatsAppNumber = "00970593617699", Status = PropertyStatus.Approved, CreatedAt = now, UpdatedAt = now },
            new Property { Id = 4, UserId = 2, Title = "مكتب تجاري مجهز", Description = "مكتب تجاري جاهز للعمل في النصر.", Price = 1200m, OperationType = OperationType.Rent, PropertyType = PropertyType.Office, Governorate = "غزة", CityAreaCamp = "النصر", DetailedAddress = "شارع النصر", Area = 85m, Rooms = 2, Bathrooms = 1, Floor = "1", Features = "تكييف, إنترنت", ContactPhone = "0593617699", WhatsAppNumber = "00970593617699", Status = PropertyStatus.Approved, CreatedAt = now, UpdatedAt = now },
            new Property { Id = 5, UserId = 2, Title = "منزل مستقل", Description = "منزل مستقل وواسع في تل السلطان.", Price = 85000m, OperationType = OperationType.Sale, PropertyType = PropertyType.House, Governorate = "رفح", CityAreaCamp = "تل السلطان", DetailedAddress = "غرب تل السلطان", Area = 200m, Rooms = 4, Bathrooms = 2, Floor = "1", Features = "حوش, تهوية ممتازة", ContactPhone = "0593617699", WhatsAppNumber = "00970593617699", Status = PropertyStatus.Approved, CreatedAt = now, UpdatedAt = now },
            new Property { Id = 6, UserId = 2, Title = "شقة عائلية", Description = "شقة عائلية مريحة في النصيرات.", Price = 60000m, OperationType = OperationType.Sale, PropertyType = PropertyType.Apartment, Governorate = "الوسطى", CityAreaCamp = "النصيرات", DetailedAddress = "المخيم الجديد", Area = 130m, Rooms = 3, Bathrooms = 1, Floor = "3", Features = "قريبة من الخدمات", ContactPhone = "0593617699", WhatsAppNumber = "00970593617699", Status = PropertyStatus.Approved, CreatedAt = now, UpdatedAt = now }
        );

        modelBuilder.Entity<PropertyImage>().HasData(
            new PropertyImage { Id = 1, PropertyId = 1, ImageUrl = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", UploadedAt = now },
            new PropertyImage { Id = 2, PropertyId = 2, ImageUrl = "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", UploadedAt = now },
            new PropertyImage { Id = 3, PropertyId = 3, ImageUrl = "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", UploadedAt = now },
            new PropertyImage { Id = 4, PropertyId = 4, ImageUrl = "https://images.unsplash.com/photo-1497366216548-37526070297c?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", UploadedAt = now },
            new PropertyImage { Id = 5, PropertyId = 5, ImageUrl = "https://images.unsplash.com/photo-1600607687920-4e2a09cf159d?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", UploadedAt = now },
            new PropertyImage { Id = 6, PropertyId = 6, ImageUrl = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", UploadedAt = now }
        );
    }
}
