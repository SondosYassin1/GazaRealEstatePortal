using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GazaRealEstatePortal.Models;
using GazaRealEstatePortal.ViewModels;

namespace GazaRealEstatePortal.Services.Interfaces;

public interface IPropertyService
{
    Task<List<Property>> GetApprovedPropertiesAsync(PropertyFilter? filter = null);
    Task<int> GetApprovedPropertiesCountAsync();
    Task<List<Property>> GetPendingPropertiesAsync();
    Task<List<Property>> GetAllPropertiesAsync(PropertyFilter? filter = null);
    Task<List<Property>> GetByUserAsync(int userId);
    Task<Property?> GetByIdAsync(int id);
    Task<List<Property>> SearchPropertiesAsync(PropertyFilter filter);
    Task<Property> AddPropertyAsync(int userId, PropertyInput input, List<IFormFile> images);
    Task<Property> UpdatePropertyAsync(int propertyId, int userId, PropertyInput input);
    Task DeletePropertyAsync(int propertyId, int userId);
}
