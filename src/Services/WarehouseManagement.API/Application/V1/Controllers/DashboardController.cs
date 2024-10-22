using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate.Specifications;
using WarehouseManagement.API.Core.Interfaces;

namespace WarehouseManagement.API.Application.V1.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Management,Auditor")]
public class DashboardController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("warehouse-status")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWarehouseStatus()
    {
        var warehouses = await _unitOfWork.Warehouses.ListAsync(new WarehousesSpec(true));

        var warehouseStatus = warehouses
            .Select(w => new
            {
                WarehouseName = w.Name,
                WarehouseLocation = $"{w.Address}, {w.City}, {w.Country}",
                ItemCount = w.Items.Count()
            });

        if (warehouseStatus is null || !warehouseStatus.Any())
        {
            return NotFound();
        }

        return Ok(warehouseStatus);
    }

    [HttpGet("top-high-items")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTopHighItems()
    {
        var topHighItems = (await _unitOfWork.WarehouseItems.ListAsync())
            .OrderByDescending(i => i.Qty)
            .Take(10)
            .Select(i => new
            {
                ItemName = i.ItemName,
                SKU = i.SKUCode,
                Quantity = i.Qty,
                Warehouse = i.Warehouse.Name
            });

        if (topHighItems == null || !topHighItems.Any())
        {
            return NotFound();
        }

        return Ok(topHighItems);
    }


    [HttpGet("top-low-items")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTopLowItems()
    {
        var topLowItems = (await _unitOfWork.WarehouseItems.ListAsync())
            .OrderBy(i => i.Qty)
            .Take(10)
            .Select(i => new
            {
                ItemName = i.ItemName,
                SKU = i.SKUCode,
                Quantity = i.Qty,
                Warehouse = i.Warehouse.Name
            });

        if (topLowItems == null || !topLowItems.Any())
        {
            return NotFound();
        }

        return Ok(topLowItems);
    }
}