using System.ComponentModel.DataAnnotations;

namespace GazaRealEstatePortal.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "الاسم الأول مطلوب")]
    [MaxLength(50, ErrorMessage = "الاسم الأول يجب ألا يتجاوز 50 حرف")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "اسم العائلة مطلوب")]
    [MaxLength(50, ErrorMessage = "اسم العائلة يجب ألا يتجاوز 50 حرف")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
    [MaxLength(150, ErrorMessage = "البريد الإلكتروني يجب ألا يتجاوز 150 حرف")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [MinLength(6, ErrorMessage = "كلمة المرور يجب ألا تقل عن 6 أحرف")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [Phone(ErrorMessage = "رقم الهاتف غير صالح")]
    public string PhoneNumber { get; set; } = string.Empty;
}
