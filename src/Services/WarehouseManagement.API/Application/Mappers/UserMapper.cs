using HappyCompany.AspNetCore.Mvc.Mapping;
using WarehouseManagement.API.Application.Dtos;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate;

namespace WarehouseManagement.API.Application.Mappers;

public interface IUserMapper : IMapper
{
    UserDto Map(User user);
}

public class UserMapper : IUserMapper
{
    public UserDto Map(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Role = user.Role,
            Email = user.Email,
            Active = user.Active,
            UserName = user.UserName
        };
    }
}