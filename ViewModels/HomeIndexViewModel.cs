using System.Collections.Generic;

namespace GazaRealEstatePortal.ViewModels;

public class HomeIndexViewModel
{
    public List<PropertyCardViewModel> FeaturedProperties { get; set; } = new List<PropertyCardViewModel>();
    public int ApprovedPropertiesCount { get; set; }
    public int ActiveUsersCount { get; set; }
}
