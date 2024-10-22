using WarehouseManagement.API.Core.AggregatesModel.UserAggregate.Enums;

namespace WarehouseManagement.API.Core.AggregatesModel.UserAggregate;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public RoleType Role { get; set; }

    public bool Active { get; set; }
}