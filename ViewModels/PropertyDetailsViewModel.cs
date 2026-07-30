using System;
using System.Collections.Generic;
using GazaRealEstatePortal.Models.Enums;

namespace GazaRealEstatePortal.ViewModels;

public class PropertyDetailsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public OperationType OperationType { get; set; }
    public PropertyType PropertyType { get; set; }
    public string Governorate { get; set; } = string.Empty;
    public string CityAreaCamp { get; set; } = string.Empty;
    public string DetailedAddress { get; set; } = string.Empty;
    public decimal Area { get; set; }
    public int Rooms { get; set; }
    public int Bathrooms { get; set; }
    public string? Floor { get; set; }
    public string? Features { get; set; }
    public string ContactPhone { get; set; } = string.Empty;
    public string WhatsAppNumber { get; set; } = string.Empty;
    public PropertyStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> ImageUrls { get; set; } = new List<string>();
    public int OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
}
