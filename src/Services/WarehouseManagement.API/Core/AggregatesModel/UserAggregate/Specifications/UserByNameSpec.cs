using Ardalis.Specification;

namespace WarehouseManagement.API.Core.AggregatesModel.UserAggregate.Specifications;

public class UserByNameSpec : SingleResultSpecification<User>
{
    public UserByNameSpec(string name)
    {
        Query.Where(p => p.UserName == name);
    }
}