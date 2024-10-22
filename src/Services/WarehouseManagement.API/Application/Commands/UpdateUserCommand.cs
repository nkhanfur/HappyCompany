using FluentValidation;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate.Enums;

namespace WarehouseManagement.API.Application.Commands;

public class UpdateUserCommand
{
    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public RoleType Role { get; set; }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(t => t.UserName)
                .NotNull().NotEmpty().MaximumLength(50);

        RuleFor(t => t.Email)
              .NotNull().NotEmpty().EmailAddress();

        RuleFor(t => t.Role)
                .NotNull().IsInEnum();
    }
}