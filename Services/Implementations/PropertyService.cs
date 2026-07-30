using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using GazaRealEstatePortal.Data;
using GazaRealEstatePortal.Models;
using GazaRealEstatePortal.Models.Enums;
using GazaRealEstatePortal.Services.Interfaces;
using GazaRealEstatePortal.ViewModels;

namespace GazaRealEstatePortal.Services.Implementations;

public class PropertyService : IPropertyService
{
    private readonly ApplicationDbContext _context;
    private readonly IImageService _imageService;

    public PropertyService(ApplicationDbContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }

    public async Task<int> GetApprovedPropertiesCountAsync()
    {
        return await _context.Properties.CountAsync(p => p.Status == PropertyStatus.Approved);
    }

    public async Task<List<Property>> GetApprovedPropertiesAsync(PropertyFilter? filter = null)
    {
        var query = _context.Properties
            .Include(p => p.Images)
            .Include(p => p.User)
            .Where(p => p.Status == PropertyStatus.Approved);

        query = ApplyFilter(query, filter);

        return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    public async Task<List<Property>> GetPendingPropertiesAsync()
    {
        return await _context.Properties
            .Include(p => p.Images)
            .Include(p => p.User)
            .Where(p => p.Status == PropertyStatus.Pending)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Property>> GetAllPropertiesAsync(PropertyFilter? filter = null)
    {
        var query = _context.Properties
            .Include(p => p.Images)
            .Include(p => p.User)
            .AsQueryable();

        query = ApplyFilter(query, filter);

        return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    public async Task<List<Property>> GetByUserAsync(int userId)
    {
        return await _context.Properties
            .Include(p => p.Images)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(int id)
    {
        return await _context.Properties
            .Include(p => p.Images)
            .Include(p => p.User)
            .Include(p => p.Review)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Property>> SearchPropertiesAsync(PropertyFilter filter)
    {
        return await GetApprovedPropertiesAsync(filter);
    }

    public async Task<Property> AddPropertyAsync(int userId, PropertyInput input, List<IFormFile> images)
    {
        var (isValid, errorMessage) = _imageService.ValidateImages(images);
        if (!isValid)
        {
            throw new ArgumentException(errorMessage);
        }

        var property = new Property
        {
            UserId = userId,
            Title = input.Title,
            Description = input.Description,
            Price = input.Price,
            OperationType = input.OperationType,
            PropertyType = input.PropertyType,
            Governorate = input.Governorate,
            CityAreaCamp = input.CityAreaCamp,
            DetailedAddress = input.DetailedAddress,
            Area = input.Area,
            Rooms = input.Rooms,
            Bathrooms = input.Bathrooms,
            Floor = input.Floor,
            Features = input.Features,
            ContactPhone = input.ContactPhone,
            WhatsAppNumber = input.WhatsAppNumber,
            Status = PropertyStatus.Pending,
            CreatedAt = DateTime.Now
        };

        _context.Properties.Add(property);
        await _context.SaveChangesAsync();

        await _imageService.SaveImagesAsync(property.Id, images);

        return property;
    }

    public async Task<Property> UpdatePropertyAsync(int propertyId, int userId, PropertyInput input)
    {
        var property = await _context.Properties.FindAsync(propertyId);
        if (property == null)
        {
            throw new KeyNotFoundException("العقار غير موجود.");
        }

        // قاعدة صارمة: تحقق Property.UserId == userId وإلا ارفض العملية
        if (property.UserId != userId)
        {
            throw new UnauthorizedAccessException("ليس لديك الصلاحية لتعديل هذا العقار.");
        }

        // قاعدة صارمة: إذا كانت الحالة السابقة Rejected، ارفض التعديل كليًا
        if (property.Status == PropertyStatus.Rejected)
        {
            throw new InvalidOperationException("لا يمكن تعديل عقار تم رفضه سابقًا من قبل الإدارة.");
        }

        // قاعدة صارمة: إذا كانت الحالة السابقة Approved، رجّعها Pending تلقائيًا بعد التعديل
        if (property.Status == PropertyStatus.Approved)
        {
            property.Status = PropertyStatus.Pending;
        }

        property.Title = input.Title;
        property.Description = input.Description;
        property.Price = input.Price;
        property.OperationType = input.OperationType;
        property.PropertyType = input.PropertyType;
        property.Governorate = input.Governorate;
        property.CityAreaCamp = input.CityAreaCamp;
        property.DetailedAddress = input.DetailedAddress;
        property.Area = input.Area;
        property.Rooms = input.Rooms;
        property.Bathrooms = input.Bathrooms;
        property.Floor = input.Floor;
        property.Features = input.Features;
        property.ContactPhone = input.ContactPhone;
        property.WhatsAppNumber = input.WhatsAppNumber;
        property.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return property;
    }

    public async Task DeletePropertyAsync(int propertyId, int userId)
    {
        var property = await _context.Properties.FindAsync(propertyId);
        if (property == null)
        {
            throw new KeyNotFoundException("العقار غير موجود.");
        }

        // تحقق من الملكية أولاً
        if (property.UserId != userId)
        {
            throw new UnauthorizedAccessException("ليس لديك الصلاحية لحذف هذا العقار.");
        }

        // احذف الصور نهائيًا
        await _imageService.DeleteImagesAsync(propertyId);

        // احذف العقار
        _context.Properties.Remove(property);
        await _context.SaveChangesAsync();
    }

    private IQueryable<Property> ApplyFilter(IQueryable<Property> query, PropertyFilter? filter)
    {
        if (filter == null)
        {
            return query;
        }

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var kw = filter.Keyword.ToLower();
            query = query.Where(p => p.Title.ToLower().Contains(kw) ||
                                     p.Description.ToLower().Contains(kw) ||
                                     p.DetailedAddress.ToLower().Contains(kw));
        }

        if (!string.IsNullOrWhiteSpace(filter.Governorate))
        {
            query = query.Where(p => p.Governorate == filter.Governorate);
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);
        }

        if (filter.OperationType.HasValue)
        {
            query = query.Where(p => p.OperationType == filter.OperationType.Value);
        }

        if (filter.PropertyType.HasValue)
        {
            query = query.Where(p => p.PropertyType == filter.PropertyType.Value);
        }

        return query;
    }
}
