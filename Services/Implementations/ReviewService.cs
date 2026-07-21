using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GazaRealEstatePortal.Data;
using GazaRealEstatePortal.Models;
using GazaRealEstatePortal.Models.Enums;
using GazaRealEstatePortal.Services.Interfaces;

namespace GazaRealEstatePortal.Services.Implementations;

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _context;

    public ReviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ApproveAsync(int propertyId, int adminId)
    {
        await ProcessReviewAsync(propertyId, adminId, ReviewDecision.Approved, PropertyStatus.Approved);
    }

    public async Task RejectAsync(int propertyId, int adminId)
    {
        await ProcessReviewAsync(propertyId, adminId, ReviewDecision.Rejected, PropertyStatus.Rejected);
    }

    private async Task ProcessReviewAsync(int propertyId, int adminId, ReviewDecision decision, PropertyStatus status)
    {
        var property = await _context.Properties.FindAsync(propertyId);
        if (property == null)
        {
            throw new KeyNotFoundException("العقار غير موجود.");
        }

        var admin = await _context.Users.FindAsync(adminId);
        if (admin == null || admin.Role != UserRole.Admin)
        {
            throw new UnauthorizedAccessException("المراجع يجب أن يكون حساب مسؤول (Admin).");
        }

        var existingReview = await _context.PropertyReviews.FirstOrDefaultAsync(pr => pr.PropertyId == propertyId);
        if (existingReview != null)
        {
            existingReview.AdminId = adminId;
            existingReview.Decision = decision;
            existingReview.ReviewedAt = DateTime.Now;
        }
        else
        {
            var review = new PropertyReview
            {
                PropertyId = propertyId,
                AdminId = adminId,
                Decision = decision,
                ReviewedAt = DateTime.Now
            };
            _context.PropertyReviews.Add(review);
        }

        property.Status = status;
        await _context.SaveChangesAsync();
    }
}
