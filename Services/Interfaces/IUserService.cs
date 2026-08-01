using System.Collections.Generic;
using System.Threading.Tasks;
using GazaRealEstatePortal.Models;
using GazaRealEstatePortal.ViewModels;

namespace GazaRealEstatePortal.Services.Interfaces;

public interface IUserService
{
    Task<User> RegisterAsync(RegisterInput input);
    Task<User?> AuthenticateAsync(string email, string password);
    Task<User> AuthenticateExternalAsync(string email, string provider, string providerId, string fullName);
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
    Task<int> GetActiveUsersCountAsync();
    Task UpdateProfileAsync(int id, string fullName, string phoneNumber, string? city, string? bio);
    Task ChangePasswordAsync(int id, string oldPassword, string newPassword);
    Task UpdateAvatarAsync(int id, string avatarUrl);
}
