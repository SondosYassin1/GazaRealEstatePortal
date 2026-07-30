using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GazaRealEstatePortal.Services.Interfaces;

public interface IImageService
{
    (bool IsValid, string ErrorMessage) ValidateImages(List<IFormFile> files);
    Task SaveImagesAsync(int propertyId, List<IFormFile> files);
    Task DeleteImagesAsync(int propertyId);
}
