using HappyCompany.AspNetCore.Mvc.Mapping;
using WarehouseManagement.API.Application.Dtos;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;

namespace WarehouseManagement.API.Application.Mappers;

public interface IWarehouseMapper : IMapper
{
    WarehouseDto Map(Warehouse warehouse);
}

public class WarehouseMapper : IWarehouseMapper
{
    public WarehouseDto Map(Warehouse warehouse)
    {
        return new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Address = warehouse.Address,
            City = warehouse.City,
            Company = new WarehouseDto.CompanyRecord
            {
                Id = warehouse.CountryId,
                Name = warehouse.Country.Name
            }
        };
    }
}