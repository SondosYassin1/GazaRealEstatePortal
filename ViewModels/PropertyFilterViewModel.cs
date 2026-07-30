using GazaRealEstatePortal.Models.Enums;

namespace GazaRealEstatePortal.ViewModels;

public class PropertyFilterViewModel
{
    public string? Governorate { get; set; }
    public string? CityAreaCamp { get; set; }
    public decimal? PriceMin { get; set; }
    public decimal? PriceMax { get; set; }
    public OperationType? OperationType { get; set; }
    public PropertyType? PropertyType { get; set; }
    public string? Keyword { get; set; }
}
