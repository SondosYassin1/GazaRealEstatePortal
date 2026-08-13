using System.ComponentModel.DataAnnotations;

namespace GazaRealEstatePortal.ViewModels;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة")]
    public string OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
    [MinLength(6, ErrorMessage = "يجب أن تتكون كلمة المرور من 6 أحرف على الأقل")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [Compare("NewPassword", ErrorMessage = "كلمتا المرور غير متطابقتين")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
