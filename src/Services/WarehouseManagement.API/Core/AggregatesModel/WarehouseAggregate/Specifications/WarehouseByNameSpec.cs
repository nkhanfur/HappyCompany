using Ardalis.Specification;

namespace WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate.Specifications;

public sealed class WarehouseByNameSpec : SingleResultSpecification<Warehouse>
{
    public WarehouseByNameSpec(string name)
    {
        Query.Where(p => p.Name == name);
    }
}