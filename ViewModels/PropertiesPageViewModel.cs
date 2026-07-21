using System.Collections.Generic;

namespace GazaRealEstatePortal.ViewModels;

public class PropertiesPageViewModel
{
    public PropertyFilterViewModel Filter { get; set; } = new PropertyFilterViewModel();
    public List<PropertyCardViewModel> Results { get; set; } = new List<PropertyCardViewModel>();
}
