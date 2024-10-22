using HappyCompany.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.API.Application.Commands;
using WarehouseManagement.API.Application.Dtos;
using WarehouseManagement.API.Application.Mappers;
using WarehouseManagement.API.Application.V1.Queries;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;
using WarehouseManagement.API.Core.Interfaces;

namespace WarehouseManagement.API.Application.V1.Controllers;

public class WarehouseItemsController : ApiControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWarehouseItemMapper _mapper;

    public WarehouseItemsController(IUnitOfWork unitOfWork, IWarehouseItemMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Management")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WarehouseItemDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseItemCommand command, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Warehouses.GetByIdAsync(command.WarehouseId, cancellationToken) is null)
        {
            return BadRequest("WAREHOUSE_NOT_FOUND", $"Warehouse not found.");
        }

        var warehouseItem = new WarehouseItem
        {
            Qty = command.Qty,
            ItemName = command.Name,
            SKUCode = command.SKUCode,
            CostPrice = command.CostPrice,
            MSRPPrice = command.MSRPPrice,
            WarehouseId = command.WarehouseId
        };

        _unitOfWork.WarehouseItems.Add(warehouseItem);
        await _unitOfWork.SaveAsync(cancellationToken);

        return CreatedAtAction(nameof(GetValue), new { warehouseItem.Id }, _mapper.Map(warehouseItem));
    }

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(WarehouseItemDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetValue(int id, CancellationToken cancellationToken)
    {
        var warehouseItem = await _unitOfWork.WarehouseItems.GetByIdAsync(id, cancellationToken);
        if (warehouseItem is null)
        {
            return NotFound();
        }

        return Ok(new { WarehouseItem = _mapper.Map(warehouseItem) });
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<WarehouseItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] WarehouseItemQuery query, CancellationToken cancellationToken)
    {
        PaginationResult? pagination = null;
        IEnumerable<WarehouseItemDto> warehouseItems;
        query.PageNumber ??= 1;
        query.PageSize ??= PaginationOptions.PageSize;

        var domainEntries = await _unitOfWork.WarehouseItems.PaginatedListAsync(query.PageNumber.Value, query.PageSize.Value, cancellationToken);
        pagination = CreatePagination(domainEntries.PageNumber, domainEntries.PageSize, domainEntries.PageCount
            , domainEntries.TotalItemCount, domainEntries.IsFirstPage, domainEntries.IsLastPage);

        warehouseItems = domainEntries.Select(_mapper.Map);

        return Ok(new { Pagination = pagination, WarehouseItems = warehouseItems });
    }
}
