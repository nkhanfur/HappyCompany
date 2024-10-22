using HappyCompany.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.API.Application.Commands;
using WarehouseManagement.API.Application.Dtos;
using WarehouseManagement.API.Application.Mappers;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate.Enums;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate.Specifications;
using WarehouseManagement.API.Core.Interfaces;

namespace WarehouseManagement.API.Application.V1.Controllers;

public class UsersController : ApiControllerBase
{
    private readonly IUserMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(IUnitOfWork unitOfWork, IUserMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Users.AnyAsync(new UserByNameSpec(command.UserName), cancellationToken))
        {
            return BadRequest("DUPLICATED_NAME", $"User  with the name '{command.UserName}' already exists.");
        }

        var country = await _unitOfWork.Users.SingleOrDefaultAsync(new UserByEmailSpec(command.Email), cancellationToken);
        if (country == null)
        {
            return BadRequest("DUPLICATED_EMAIL", $"User  with the email '{command.Email}' already exists.");
        }

        var user = new User
        {
            Role = command.Role,
            Email = command.Email,
            Active = command.Active,
            Password = command.Password,
            UserName = command.UserName,
        };

        _unitOfWork.Users.Add(user);
        await _unitOfWork.SaveAsync(cancellationToken);

        return CreatedAtAction(nameof(GetValue), new { user.Id }, _mapper.Map(user));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return NotFound();
        }

        var sameUserByName = await _unitOfWork.Users.SingleOrDefaultAsync(new UserByNameSpec(command.UserName), cancellationToken);
        if (sameUserByName != null && sameUserByName.Id != user.Id)
        {
            return BadRequest("DUPLICATED_NAME", $"User with the name '{command.UserName}' already exists.");
        }

        var sameUserByEmail = await _unitOfWork.Users.SingleOrDefaultAsync(new UserByEmailSpec(command.Email), cancellationToken);
        if (sameUserByEmail != null && sameUserByEmail.Id != user.Id)
        {
            return BadRequest("DUPLICATED_NAME", $"User with the email '{command.Email}' already exists.");
        }

        user.UserName = command.UserName;
        user.Email = command.Email;
        user.Role = command.Role;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveAsync(cancellationToken);

        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetValue(int id, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(new { User = _mapper.Map(user) });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return NotFound();
        }

        if (user.Role is RoleType.Admin)
        {
            return BadRequest("INVALID_ROLE_REMOVED", $"The user with role '{user.Role}' cannot be deleted.");
        }

        _unitOfWork.Users.Remove(user);
        await _unitOfWork.SaveAsync(cancellationToken);

        return NoContent();
    }
}