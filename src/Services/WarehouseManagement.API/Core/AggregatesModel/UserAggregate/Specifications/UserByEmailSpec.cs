using Ardalis.Specification;

namespace WarehouseManagement.API.Core.AggregatesModel.UserAggregate.Specifications;

public class UserByEmailSpec : SingleResultSpecification<User>
{
    public UserByEmailSpec(string email)
    {
        Query.Where(p => p.Email == email);
    }
}