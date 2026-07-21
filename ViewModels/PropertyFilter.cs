using GazaRealEstatePortal.Models.Enums;

namespace GazaRealEstatePortal.ViewModels;

public class PropertyFilter
{
    public string? Keyword { get; set; }
    public string? Governorate { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public OperationType? OperationType { get; set; }
    public PropertyType? PropertyType { get; set; }
}
