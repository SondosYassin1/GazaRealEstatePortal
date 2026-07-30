using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using GazaRealEstatePortal.Models;
using GazaRealEstatePortal.Models.Enums;
using GazaRealEstatePortal.Services.Interfaces;
using GazaRealEstatePortal.ViewModels;

namespace GazaRealEstatePortal.Controllers;

public class HomeController : Controller
{
    private readonly IPropertyService _propertyService;
    private readonly IUserService _userService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IPropertyService propertyService, IUserService userService, ILogger<HomeController> logger)
    {
        _propertyService = propertyService;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var properties = await _propertyService.GetApprovedPropertiesAsync();
        
        var viewModels = properties
            .OrderByDescending(p => p.CreatedAt)
            .Take(6)
            .Select(p => new PropertyCardViewModel
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

        var approvedCount = await _propertyService.GetApprovedPropertiesCountAsync();
        var activeUsersCount = await _userService.GetActiveUsersCountAsync();

        var viewModel = new HomeIndexViewModel
        {
            FeaturedProperties = viewModels,
            ApprovedPropertiesCount = approvedCount,
            ActiveUsersCount = activeUsersCount
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Properties(PropertyFilterViewModel Filter)
    {
        var filter = new PropertyFilter
        {
            Keyword = Filter.Keyword,
            Governorate = Filter.Governorate,
            MinPrice = Filter.PriceMin,
            MaxPrice = Filter.PriceMax,
            OperationType = Filter.OperationType,
            PropertyType = Filter.PropertyType
        };

        var properties = await _propertyService.SearchPropertiesAsync(filter);

        // Map to Card ViewModels
        var viewModels = properties
            .Where(p => p.Status == PropertyStatus.Approved) // Ensure only approved are shown publicly here
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PropertyCardViewModel
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

        return View(pageViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> PropertyDetails(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        
        if (property == null)
        {
            return NotFound();
        }

        // Access control check for non-approved properties
        if (property.Status != PropertyStatus.Approved)
        {
            bool isOwner = User.Identity?.IsAuthenticated == true && 
                           User.FindFirst(ClaimTypes.NameIdentifier)?.Value == property.UserId.ToString();
            bool isAdmin = User.IsInRole("Admin");

            if (!isOwner && !isAdmin)
            {
                return NotFound();
            }
        }

        var viewModel = new PropertyDetailsViewModel
        {
            Id = property.Id,
            Title = property.Title,
            Description = property.Description,
            Price = property.Price,
            OperationType = property.OperationType,
            PropertyType = property.PropertyType,
            Governorate = property.Governorate,
            CityAreaCamp = property.CityAreaCamp,
            DetailedAddress = property.DetailedAddress,
            ContactPhone = property.ContactPhone,
            WhatsAppNumber = property.WhatsAppNumber,
            Area = property.Area,
            Rooms = property.Rooms,
            Bathrooms = property.Bathrooms,
            Floor = property.Floor,
            Features = property.Features,
            Status = property.Status,
            CreatedAt = property.CreatedAt,
            ImageUrls = property.Images.Select(i => i.ImageUrl).ToList(),
            OwnerId = property.UserId,
            OwnerName = property.User?.FullName ?? "غير معروف"
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? id)
    {
        if (id == 404)
        {
            return View("NotFound");
        }
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
