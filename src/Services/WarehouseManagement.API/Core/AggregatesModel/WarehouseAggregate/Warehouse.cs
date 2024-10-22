using System.Diagnostics.Metrics;

namespace WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;

public class Warehouse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public int CountryId { get; set; }

    public Country Country { get; set; } = null!;

    public List<WarehouseItem> Items { get; set; } = [];
}