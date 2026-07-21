using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GazaRealEstatePortal.Models.Enums;

namespace GazaRealEstatePortal.Models;

public class PropertyReview
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PropertyId { get; set; }

    [ForeignKey(nameof(PropertyId))]
    public virtual Property Property { get; set; } = null!;

    [Required]
    public int AdminId { get; set; }

    [ForeignKey(nameof(AdminId))]
    public virtual User Admin { get; set; } = null!;

    [Required]
    public ReviewDecision Decision { get; set; }

    public DateTime ReviewedAt { get; set; } = DateTime.Now;
}
