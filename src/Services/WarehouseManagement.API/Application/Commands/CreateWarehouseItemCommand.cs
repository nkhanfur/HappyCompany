using FluentValidation;

namespace WarehouseManagement.API.Application.Commands;

public class CreateWarehouseItemCommand
{
    public string Name { get; set; } = null!;

    public string? SKUCode { get; set; }

    public int Qty { get; set; }

    public decimal CostPrice { get; set; }

    public decimal? MSRPPrice { get; set; }

    public int WarehouseId { get; set; }
}

public class CreateWarehouseItemCommandValidator : AbstractValidator<CreateWarehouseItemCommand>
{
    public CreateWarehouseItemCommandValidator()
    {
        RuleFor(t => t.Name)
                .NotNull().NotEmpty().MaximumLength(50);

        RuleFor(t => t.SKUCode)
                .MaximumLength(50);

        RuleFor(t => t.Qty).NotNull().GreaterThanOrEqualTo(1);

        RuleFor(t => t.CostPrice)
              .NotNull().InclusiveBetween(0, 10000);

        RuleFor(t => t.MSRPPrice)
               .InclusiveBetween(0, 10000);
    }
}