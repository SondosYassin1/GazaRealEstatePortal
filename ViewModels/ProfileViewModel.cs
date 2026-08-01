using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
namespace GazaRealEstatePortal.ViewModels;

public class ProfileViewModel
{
    [Required(ErrorMessage = "الاسم الكامل مطلوب")]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [Phone(ErrorMessage = "رقم هاتف غير صالح")]
    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty; // Read-only

    [MaxLength(50)]
    public string? City { get; set; }

    [MaxLength(500)]
    public string? Bio { get; set; }

    public string? AvatarUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public int PropertiesCount { get; set; }
    public List<PropertyCardViewModel> RecentProperties { get; set; } = new List<PropertyCardViewModel>();
}
