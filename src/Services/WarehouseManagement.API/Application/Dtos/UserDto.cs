using WarehouseManagement.API.Core.AggregatesModel.UserAggregate.Enums;

namespace WarehouseManagement.API.Application.Dtos;

public class UserDto
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public RoleType Role { get; set; }

    public bool Active { get; set; }
}