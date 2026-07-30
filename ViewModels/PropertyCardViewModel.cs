using System;
using GazaRealEstatePortal.Models.Enums;

namespace GazaRealEstatePortal.ViewModels;

public class PropertyCardViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public OperationType OperationType { get; set; }
    public PropertyType PropertyType { get; set; }
    public string Governorate { get; set; } = string.Empty;
    public string CityAreaCamp { get; set; } = string.Empty;
    public string MainImageUrl { get; set; } = string.Empty;
    public PropertyStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
