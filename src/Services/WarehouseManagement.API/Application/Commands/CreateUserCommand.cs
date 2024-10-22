using FluentValidation;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate.Enums;

namespace WarehouseManagement.API.Application.Commands;

public class CreateUserCommand
{
    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public RoleType Role { get; set; }

    public bool Active { get; set; }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(t => t.UserName)
                .NotNull().NotEmpty().MaximumLength(50);

        RuleFor(t => t.Email)
              .NotNull().NotEmpty().EmailAddress();

        RuleFor(t => t.Password)
                .NotNull().NotEmpty().MaximumLength(50);

        RuleFor(t => t.Role)
                .NotNull().IsInEnum();

        RuleFor(t => t.Active)
            .NotNull();
    }
}