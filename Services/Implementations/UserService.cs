using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GazaRealEstatePortal.Data;
using GazaRealEstatePortal.Models;
using GazaRealEstatePortal.Models.Enums;
using GazaRealEstatePortal.Services.Interfaces;
using GazaRealEstatePortal.ViewModels;

namespace GazaRealEstatePortal.Services.Implementations;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> RegisterAsync(RegisterInput input)
    {
        var emailExists = await _context.Users.AnyAsync(u => u.Email.ToLower() == input.Email.ToLower());
        if (emailExists)
        {
            throw new ArgumentException("البريد الإلكتروني مستخدم بالفعل.");
        }

        var user = new User
        {
            FullName = input.FullName,
            Email = input.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.Password),
            PhoneNumber = input.PhoneNumber,
            Role = UserRole.RegisteredUser,
            CreatedAt = DateTime.Now,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.IsActive);
        if (user == null)
        {
            return null;
        }

        if (user.PasswordHash == null)
        {
            throw new ArgumentException($"هذا الحساب مسجل عبر {user.ExternalProvider ?? "طريقة أخرى"}، الرجاء تسجيل الدخول بنفس الطريقة.");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return null;
        }

        return user;
    }

    public async Task<User> AuthenticateExternalAsync(string email, string provider, string providerId, string fullName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        
        if (user != null)
        {
            // Link account if not linked
            if (string.IsNullOrEmpty(user.ExternalProvider) || string.IsNullOrEmpty(user.ExternalProviderId))
            {
                user.ExternalProvider = provider;
                user.ExternalProviderId = providerId;
                await _context.SaveChangesAsync();
            }
            return user;
        }

        // Create new account
        user = new User
        {
            FullName = string.IsNullOrEmpty(fullName) ? "مستخدم جديد" : fullName,
            Email = email,
            PasswordHash = null,
            PhoneNumber = "", // Not provided by default oauth, can be empty
            Role = UserRole.RegisteredUser,
            CreatedAt = DateTime.Now,
            IsActive = true,
            ExternalProvider = provider,
            ExternalProviderId = providerId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<int> GetActiveUsersCountAsync()
    {
        return await _context.Users.CountAsync(u => u.IsActive);
    }

    public async Task UpdateProfileAsync(int id, string fullName, string phoneNumber, string? city, string? bio)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("المستخدم غير موجود.");
        }

        user.FullName = fullName;
        user.PhoneNumber = phoneNumber;
        user.City = city;
        user.Bio = bio;

        await _context.SaveChangesAsync();
    }

    public async Task ChangePasswordAsync(int id, string oldPassword, string newPassword)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("المستخدم غير موجود.");
        }

        if (user.PasswordHash == null)
        {
            throw new ArgumentException("هذا الحساب مسجل عبر طريقة أخرى ولا يمكن تغيير كلمة مروره.");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new ArgumentException("كلمة المرور الحالية غير صحيحة.");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAvatarAsync(int id, string avatarUrl)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("المستخدم غير موجود.");
        }

        user.AvatarUrl = avatarUrl;
        await _context.SaveChangesAsync();
    }
}
