using Ardalis.Specification;

namespace WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate.Specifications;

public class WarehousesSpec : Specification<Warehouse>
{
    public WarehousesSpec(bool includeItems)
    {
        if (includeItems)
        {
            Query.Include(q => q.Items);
        }
    }
}