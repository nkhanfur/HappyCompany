using HappyCompany.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.API.Application.Commands;
using WarehouseManagement.API.Application.Dtos;
using WarehouseManagement.API.Application.Mappers;
using WarehouseManagement.API.Application.V1.Queries;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate.Specifications;
using WarehouseManagement.API.Core.Interfaces;

namespace WarehouseManagement.API.Application.V1.Controllers;

public class WarehousesController : ApiControllerBase
{
    private readonly IWarehouseMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public WarehousesController(IUnitOfWork unitOfWork, IWarehouseMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseCommand command, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Warehouses.AnyAsync(new WarehouseByNameSpec(command.Name), cancellationToken))
        {
            return BadRequest("DUPLICATED_NAME", $"Warehouse  with the '{command.Name}' already exists.");
        }

        var country = await _unitOfWork.Countries.SingleOrDefaultAsync(new CountryByNameSpec(command.Country), cancellationToken);
        if (country == null)
        {
            return BadRequest("COUNTRY_NOT_FOUND", $"Country  with the '{command.Name}' not exists.");
        }

        var warehouse = new Warehouse
        {
            Name = command.Name,
            Address = command.Address,
            City = command.City,
            CountryId = country.Id
        };

        _unitOfWork.Warehouses.Add(warehouse);
        await _unitOfWork.SaveAsync(cancellationToken);

        return CreatedAtAction(nameof(GetValue), new { warehouse.Id }, _mapper.Map(warehouse));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Management")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetValue(int id, CancellationToken cancellationToken)
    {
        var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(id, cancellationToken);
        if (warehouse is null)
        {
            return NotFound();
        }

        return Ok(new { Warehouse = _mapper.Map(warehouse) });
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Management")]
    [ProducesResponseType(typeof(IEnumerable<WarehouseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] WarehouseQuery query, CancellationToken cancellationToken)
    {
        PaginationResult? pagination = null;
        IEnumerable<WarehouseDto> warehouses;
        query.PageNumber ??= 1;
        query.PageSize ??= PaginationOptions.PageSize;

        var domainEntries = await _unitOfWork.Warehouses.PaginatedListAsync(query.PageNumber.Value, query.PageSize.Value, cancellationToken);
        pagination = CreatePagination(domainEntries.PageNumber, domainEntries.PageSize, domainEntries.PageCount
            , domainEntries.TotalItemCount, domainEntries.IsFirstPage, domainEntries.IsLastPage);

        warehouses = domainEntries.Select(_mapper.Map);

        return Ok(new { Pagination = pagination, Warehouses = warehouses });
    }

    [Authorize(Roles = "Auditor")]
    [HttpGet("warehouse-status")]
    public async Task<IActionResult> GetWarehouseStatus(CancellationToken cancellationToken)
    {
        var warehouses = await _unitOfWork.Warehouses.ListAsync(cancellationToken);
        var status = warehouses.Select(wh => new
        {
            WarehouseName = wh.Name,
            ItemCount = wh.Items.Count
        });

        return Ok(status);
    }
}