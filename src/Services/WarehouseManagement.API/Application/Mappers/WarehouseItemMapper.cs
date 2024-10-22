using HappyCompany.AspNetCore.Mvc.Mapping;
using WarehouseManagement.API.Application.Dtos;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;

namespace WarehouseManagement.API.Application.Mappers;

public interface IWarehouseItemMapper : IMapper
{
    WarehouseItemDto Map(WarehouseItem item);
}
public class WarehouseItemMapper : IWarehouseItemMapper
{
    public WarehouseItemDto Map(WarehouseItem item)
    {
        return new WarehouseItemDto
        {
            Id = item.Id,
            Qty = item.Qty,
            Name = item.ItemName,
            SKUCode = item.SKUCode,
            CostPrice = item.CostPrice,
            MSRPPrice = item.MSRPPrice,
            Warehouse = new WarehouseItemDto.WarehouseRecord
            {
                Id = item.WarehouseId,
                Name = item.Warehouse.Name,
                Address = item.Warehouse.Address
            }
        };
    }
}