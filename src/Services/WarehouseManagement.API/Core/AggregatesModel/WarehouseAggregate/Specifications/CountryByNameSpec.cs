using Ardalis.Specification;

namespace WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate.Specifications;

public sealed class CountryByNameSpec : SingleResultSpecification<Country>
{
    public CountryByNameSpec(string name)
    {
        Query.Where(p => p.Name == name);
    }
}
