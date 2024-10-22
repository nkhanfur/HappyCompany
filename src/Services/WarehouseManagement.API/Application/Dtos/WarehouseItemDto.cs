using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;

namespace WarehouseManagement.API.Application.Dtos;

public class WarehouseItemDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? SKUCode { get; set; }

    public int Qty { get; set; }

    public decimal CostPrice { get; set; }

    public decimal? MSRPPrice { get; set; }

    public WarehouseRecord Warehouse { get; set; }

    public record struct WarehouseRecord(int Id, string Name,string Address);
}