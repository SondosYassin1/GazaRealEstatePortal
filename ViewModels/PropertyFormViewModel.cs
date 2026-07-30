using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using GazaRealEstatePortal.Models.Enums;

namespace GazaRealEstatePortal.ViewModels;

public class PropertyFormViewModel
{
    [Required(ErrorMessage = "العنوان مطلوب")]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "الوصف مطلوب")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "السعر مطلوب")]
    [Range(0, double.MaxValue, ErrorMessage = "يجب أن يكون السعر رقماً موجباً")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "نوع العملية مطلوب")]
    public OperationType OperationType { get; set; }

    [Required(ErrorMessage = "نوع العقار مطلوب")]
    public PropertyType PropertyType { get; set; }

    [Required(ErrorMessage = "المحافظة مطلوبة")]
    public string Governorate { get; set; } = string.Empty;

    [Required(ErrorMessage = "المدينة/المنطقة/المخيم مطلوب")]
    public string CityAreaCamp { get; set; } = string.Empty;

    [Required(ErrorMessage = "العنوان التفصيلي مطلوب")]
    public string DetailedAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "المساحة مطلوبة")]
    [Range(1, double.MaxValue, ErrorMessage = "المساحة يجب أن تكون أكبر من صفر")]
    public decimal Area { get; set; }

    [Required(ErrorMessage = "عدد الغرف مطلوب")]
    [Range(0, 100, ErrorMessage = "عدد الغرف يجب أن يكون بين 0 و 100")]
    public int Rooms { get; set; }

    [Required(ErrorMessage = "عدد الحمامات مطلوب")]
    [Range(0, 100, ErrorMessage = "عدد الحمامات يجب أن يكون بين 0 و 100")]
    public int Bathrooms { get; set; }

    public string? Floor { get; set; }

    public List<string> SelectedFeatures { get; set; } = new List<string>();

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [RegularExpression(@"^05\d{8}$", ErrorMessage = "رقم الهاتف يجب أن يبدأ بـ 05 ويتكون من 10 أرقام بالكامل، مثال: 0593617699")]
    public string ContactPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "رقم الواتساب مطلوب")]
    [RegularExpression(@"^(00970|00972)\d{9}$", ErrorMessage = "رقم الواتساب يجب أن يبدأ بـ 00970 أو 00972 ويتكون من 14 رقم بالكامل، مثال: 00970593617699")]
    public string WhatsAppNumber { get; set; } = string.Empty;

    public List<IFormFile> Images { get; set; } = new List<IFormFile>();

    // For Edit Mode, we might want to display existing image URLs, but the prompt just says PropertyFormViewModel. 
    // We can add a property for existing images.
    public List<string> ExistingImageUrls { get; set; } = new List<string>();
}
