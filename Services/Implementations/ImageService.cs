using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using GazaRealEstatePortal.Data;
using GazaRealEstatePortal.Models;
using GazaRealEstatePortal.Services.Interfaces;

namespace GazaRealEstatePortal.Services.Implementations;

public class ImageService : IImageService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string _uploadsFolder;

    public ImageService(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
    }

    public (bool IsValid, string ErrorMessage) ValidateImages(List<IFormFile> files)
    {
        if (files == null || files.Count < 3 || files.Count > 5)
        {
            return (false, "يجب رفع ما بين 3 إلى 5 صور.");
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        const long maxFileSize = 5 * 1024 * 1024; // 5MB

        foreach (var file in files)
        {
            if (file == null || file.Length == 0)
            {
                return (false, "إحدى الصور المرفوعة غير صالحة أو فارغة.");
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                return (false, $"صيغة الملف {file.FileName} غير مدعومة. الصيغ المسموحة هي: jpg, jpeg, png.");
            }

            if (file.Length > maxFileSize)
            {
                return (false, $"حجم الصورة {file.FileName} يتجاوز الحد الأقصى المسموح به وهو 5 ميجابايت.");
            }
        }

        return (true, string.Empty);
    }

    public async Task SaveImagesAsync(int propertyId, List<IFormFile> files)
    {
        if (!Directory.Exists(_uploadsFolder))
        {
            Directory.CreateDirectory(_uploadsFolder);
        }

        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(_uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var propertyImage = new PropertyImage
            {
                PropertyId = propertyId,
                ImageUrl = "/uploads/" + uniqueFileName,
                UploadedAt = DateTime.Now
            };

            _context.PropertyImages.Add(propertyImage);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteImagesAsync(int propertyId)
    {
        var images = await _context.PropertyImages
            .Where(pi => pi.PropertyId == propertyId)
            .ToListAsync();

        foreach (var image in images)
            {
                var fileName = Path.GetFileName(image.ImageUrl);
                var filePath = Path.Combine(_uploadsFolder, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _context.PropertyImages.Remove(image);
            }

            await _context.SaveChangesAsync();
        }
    }
