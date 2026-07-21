using System.ComponentModel.DataAnnotations;
using GazaRealEstatePortal.Models.Enums;

namespace GazaRealEstatePortal.ViewModels;

public class PropertyInput
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
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

    public decimal Area { get; set; }
    public int Rooms { get; set; }
    public int Bathrooms { get; set; }
    public string? Floor { get; set; }
    public string? Features { get; set; }

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [RegularExpression(@"^05\d{8}$", ErrorMessage = "رقم الهاتف يجب أن يبدأ بـ 05 ويتكون من 10 أرقام بالكامل، مثال: 0593617699")]
    public string ContactPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "رقم الواتساب مطلوب")]
    [RegularExpression(@"^(00970|00972)\d{9}$", ErrorMessage = "رقم الواتساب يجب أن يبدأ بـ 00970 أو 00972 ويتكون من 14 رقم بالكامل، مثال: 00970593617699")]
    public string WhatsAppNumber { get; set; } = string.Empty;
}
