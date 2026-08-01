using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GazaRealEstatePortal.Models.Enums;

namespace GazaRealEstatePortal.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    public string? PasswordHash { get; set; }

    public string? ExternalProvider { get; set; }
    public string? ExternalProviderId { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.RegisteredUser;

    [MaxLength(50)]
    public string? City { get; set; }

    [MaxLength(500)]
    public string? Bio { get; set; }

    [MaxLength(255)]
    public string? AvatarUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    public virtual ICollection<PropertyReview> ReviewsMade { get; set; } = new List<PropertyReview>();
}
