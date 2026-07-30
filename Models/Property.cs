using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GazaRealEstatePortal.Models.Enums;

namespace GazaRealEstatePortal.Models;

public class Property
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public OperationType OperationType { get; set; }

    [Required]
    public PropertyType PropertyType { get; set; }

    [Required]
    public string Governorate { get; set; } = string.Empty;

    [Required]
    public string CityAreaCamp { get; set; } = string.Empty;

    [Required]
    public string DetailedAddress { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Area { get; set; }

    [Required]
    public int Rooms { get; set; }

    [Required]
    public int Bathrooms { get; set; }

    public string? Floor { get; set; }

    public string? Features { get; set; }

    [Required]
    [Phone]
    public string ContactPhone { get; set; } = string.Empty;

    [Required]
    public string WhatsAppNumber { get; set; } = string.Empty;

    public PropertyStatus Status { get; set; } = PropertyStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    
    public virtual PropertyReview? Review { get; set; }
}
