using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GazaRealEstatePortal.Services.Interfaces;
using GazaRealEstatePortal.ViewModels;
using GazaRealEstatePortal.Models.Enums;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GazaRealEstatePortal.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IPropertyService _propertyService;
    private readonly IReviewService _reviewService;
    private readonly IUserService _userService;

    public AdminController(IPropertyService propertyService, IReviewService reviewService, IUserService userService)
    {
        _propertyService = propertyService;
        _reviewService = reviewService;
        _userService = userService;
    }

    private int GetCurrentAdminId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null && int.TryParse(claim.Value, out int id) ? id : 0;
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var properties = await _propertyService.GetAllPropertiesAsync();
        var users = await _userService.GetAllAsync();
        var pendingProperties = await _propertyService.GetPendingPropertiesAsync();

        var viewModel = new AdminDashboardViewModel
        {
            TotalProperties = properties.Count,
            PendingCount = properties.Count(p => p.Status == PropertyStatus.Pending),
            ApprovedCount = properties.Count(p => p.Status == PropertyStatus.Approved),
            RejectedCount = properties.Count(p => p.Status == PropertyStatus.Rejected),
            TotalUsers = users.Count,
            LastPendingProperties = pendingProperties.Take(5).Select(p => new PropertyCardViewModel
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
    public async Task<IActionResult> PendingProperties()
    {
        var pendingProperties = await _propertyService.GetPendingPropertiesAsync();
        
        var viewModels = pendingProperties.Select(p => new PropertyCardViewModel
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
        }).ToList();

        var pageViewModel = new PropertiesPageViewModel
        {
            Filter = new PropertyFilterViewModel(),
            Results = viewModels
        };

        ViewBag.ShowFilter = false;

        return View("Properties", pageViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Properties(PropertyFilterViewModel Filter)
    {
        // Using all properties for admin search
        var filter = new PropertyFilter
        {
            Keyword = Filter.Keyword,
            Governorate = Filter.Governorate,
            MinPrice = Filter.PriceMin,
            MaxPrice = Filter.PriceMax,
            OperationType = Filter.OperationType,
            PropertyType = Filter.PropertyType
        };

        var properties = await _propertyService.GetAllPropertiesAsync(filter);

        var viewModels = properties.OrderByDescending(p => p.CreatedAt).Select(p => new PropertyCardViewModel
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
        }).ToList();

        var pageViewModel = new PropertiesPageViewModel
        {
            Filter = Filter,
            Results = viewModels
        };

        ViewBag.ShowFilter = true;

        return View(pageViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveProperty(int id)
    {
        int adminId = GetCurrentAdminId();
        try
        {
            await _reviewService.ApproveAsync(id, adminId);
            TempData["SuccessMessage"] = "تمت الموافقة على العقار بنجاح.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        
        return Redirect(Request.Headers["Referer"].ToString() ?? Url.Action("Dashboard") ?? "/");
    }

    [HttpPost]
    public async Task<IActionResult> RejectProperty(int id)
    {
        int adminId = GetCurrentAdminId();
        try
        {
            await _reviewService.RejectAsync(id, adminId);
            TempData["SuccessMessage"] = "تم رفض العقار بنجاح.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return Redirect(Request.Headers["Referer"].ToString() ?? Url.Action("Dashboard") ?? "/");
    }

    [HttpGet]
    public async Task<IActionResult> Users()
    {
        var users = await _userService.GetAllAsync();
        // Just pass the List<User> to the view directly since it's a read-only view and we don't need a specific ViewModel for now.
        return View(users);
    }
}
