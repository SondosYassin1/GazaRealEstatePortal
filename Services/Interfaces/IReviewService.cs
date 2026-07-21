using System.Threading.Tasks;

namespace GazaRealEstatePortal.Services.Interfaces;

public interface IReviewService
{
    Task ApproveAsync(int propertyId, int adminId);
    Task RejectAsync(int propertyId, int adminId);
}
