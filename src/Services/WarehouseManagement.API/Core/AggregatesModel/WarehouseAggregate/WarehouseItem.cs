namespace WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;

public class WarehouseItem
{
    public int Id { get; set; }

    public string ItemName { get; set; } = null!;

    public string? SKUCode { get; set; } 

    public int Qty { get; set; }

    public decimal CostPrice { get; set; }

    public decimal? MSRPPrice { get; set; }

    public int WarehouseId { get; set; }

    public Warehouse Warehouse { get; set; } = new();
}