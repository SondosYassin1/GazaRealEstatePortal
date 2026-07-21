using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GazaRealEstatePortal.Services.Interfaces;
using GazaRealEstatePortal.ViewModels;
using GazaRealEstatePortal.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GazaRealEstatePortal.Controllers;

[Authorize(Roles = "RegisteredUser")]
public class PropertiesController : Controller
{
    private readonly IPropertyService _propertyService;
    private readonly IUserService _userService;

    public PropertiesController(IPropertyService propertyService, IUserService userService)
    {
        _propertyService = propertyService;
        _userService = userService;
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null && int.TryParse(claim.Value, out int id) ? id : 0;
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        int userId = GetCurrentUserId();
        var userProperties = await _propertyService.GetByUserAsync(userId);

        var viewModel = new UserDashboardViewModel
        {
            PendingCount = userProperties.Count(p => p.Status == PropertyStatus.Pending),
            ApprovedCount = userProperties.Count(p => p.Status == PropertyStatus.Approved),
            RejectedCount = userProperties.Count(p => p.Status == PropertyStatus.Rejected),
            LastProperties = userProperties.Take(5).Select(p => new PropertyCardViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                OperationType = p.OperationType,
                PropertyType = p.PropertyType,
                Governorate = p.Governorate,
                CityAreaCamp = p.CityAreaCamp,
                MainImageUrl = p.Images.FirstOrDefault()?.ImageUrl ?? "/images/default-property.jpg",
                Status = p.Status,
                CreatedAt = p.CreatedAt
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult AddProperty()
    {
        return View(new PropertyFormViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> AddProperty(PropertyFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        int userId = GetCurrentUserId();
        
        var input = new PropertyInput
        {
            Title = model.Title,
            Description = model.Description,
            Price = model.Price,
            OperationType = model.OperationType,
            PropertyType = model.PropertyType,
            Governorate = model.Governorate,
            CityAreaCamp = model.CityAreaCamp,
            DetailedAddress = model.DetailedAddress,
            Area = model.Area,
            Rooms = model.Rooms,
            Bathrooms = model.Bathrooms,
            Floor = model.Floor,
            Features = model.SelectedFeatures != null && model.SelectedFeatures.Any() 
                       ? string.Join(",", model.SelectedFeatures) 
                       : null,
            ContactPhone = model.ContactPhone,
            WhatsAppNumber = model.WhatsAppNumber
        };

        try
        {
            await _propertyService.AddPropertyAsync(userId, input, model.Images);
            TempData["SuccessMessage"] = "تمت إضافة العقار بنجاح وهو الآن قيد المراجعة.";
            return RedirectToAction(nameof(Dashboard));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("Images", ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditProperty(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property == null) return NotFound();

        int userId = GetCurrentUserId();
        if (property.UserId != userId) return Forbid();

        if (property.Status == PropertyStatus.Rejected)
        {
            TempData["ErrorMessage"] = "لا يمكن تعديل عقار مرفوض.";
            return RedirectToAction(nameof(Dashboard));
        }

        var model = new PropertyFormViewModel
        {
            Title = property.Title,
            Description = property.Description,
            Price = property.Price,
            OperationType = property.OperationType,
            PropertyType = property.PropertyType,
            Governorate = property.Governorate,
            CityAreaCamp = property.CityAreaCamp,
            DetailedAddress = property.DetailedAddress,
            Area = property.Area,
            Rooms = property.Rooms,
            Bathrooms = property.Bathrooms,
            Floor = property.Floor,
            SelectedFeatures = !string.IsNullOrEmpty(property.Features) 
                               ? property.Features.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() 
                               : new List<string>(),
            ContactPhone = property.ContactPhone,
            WhatsAppNumber = property.WhatsAppNumber,
            ExistingImageUrls = property.Images.Select(i => i.ImageUrl).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditProperty(int id, PropertyFormViewModel model)
    {
        ModelState.Remove("Images");

        if (!ModelState.IsValid)
            return View(model);

        int userId = GetCurrentUserId();
        
        var input = new PropertyInput
        {
            Title = model.Title,
            Description = model.Description,
            Price = model.Price,
            OperationType = model.OperationType,
            PropertyType = model.PropertyType,
            Governorate = model.Governorate,
            CityAreaCamp = model.CityAreaCamp,
            DetailedAddress = model.DetailedAddress,
            Area = model.Area,
            Rooms = model.Rooms,
            Bathrooms = model.Bathrooms,
            Floor = model.Floor,
            Features = model.SelectedFeatures != null && model.SelectedFeatures.Any() 
                       ? string.Join(",", model.SelectedFeatures) 
                       : null,
            ContactPhone = model.ContactPhone,
            WhatsAppNumber = model.WhatsAppNumber
        };

        try
        {
            await _propertyService.UpdatePropertyAsync(id, userId, input);
            TempData["SuccessMessage"] = "تم تعديل العقار بنجاح.";
            return RedirectToAction(nameof(Dashboard));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProperty(int id)
    {
        int userId = GetCurrentUserId();
        try
        {
            await _propertyService.DeletePropertyAsync(id, userId);
            TempData["SuccessMessage"] = "تم حذف العقار بنجاح.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Dashboard));
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        int userId = GetCurrentUserId();
        var user = await _userService.GetByIdAsync(userId);
        if (user == null) return NotFound();

        var model = new ProfileViewModel
        {
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        int userId = GetCurrentUserId();
        await _userService.UpdateProfileAsync(userId, model.FullName, model.PhoneNumber);
        
        TempData["SuccessMessage"] = "تم تحديث البيانات بنجاح.";
        return RedirectToAction(nameof(Profile));
    }
}
