using System.ComponentModel.DataAnnotations;

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
}
