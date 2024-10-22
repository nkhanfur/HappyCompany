using Microsoft.AspNetCore.Identity;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate;

namespace WarehouseManagement.API.Infrastructure.Services;

public class PasswordService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }
}