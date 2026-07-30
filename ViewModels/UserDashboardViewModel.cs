using System.Collections.Generic;

namespace GazaRealEstatePortal.ViewModels;

public class UserDashboardViewModel
{
    public int PendingCount { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
    public List<PropertyCardViewModel> LastProperties { get; set; } = new List<PropertyCardViewModel>();
}
