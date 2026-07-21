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

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.RegisteredUser;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    public virtual ICollection<PropertyReview> ReviewsMade { get; set; } = new List<PropertyReview>();
}
