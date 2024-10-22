using FluentValidation;

namespace WarehouseManagement.API.Application.Commands;

public class CreateWarehouseCommand
{
    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;
}

public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator()
    {
        RuleFor(t => t.Name)
                .NotNull().NotEmpty().MaximumLength(50);

        RuleFor(t => t.Address)
              .NotNull().NotEmpty().MaximumLength(250);

        RuleFor(t => t.City)
                .NotNull().NotEmpty().MaximumLength(50);

        RuleFor(t => t.Country)
                .NotNull().NotEmpty().MaximumLength(50);
    }
}