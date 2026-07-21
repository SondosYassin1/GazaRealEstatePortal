using System.Collections.Generic;

namespace GazaRealEstatePortal.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalProperties { get; set; }
    public int PendingCount { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
    public int TotalUsers { get; set; }
    
    public List<PropertyCardViewModel> LastPendingProperties { get; set; } = new List<PropertyCardViewModel>();
}
