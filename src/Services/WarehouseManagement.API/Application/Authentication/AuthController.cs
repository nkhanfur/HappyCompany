using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WarehouseManagement.API.Application.Commands;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate.Specifications;
using WarehouseManagement.API.Core.Interfaces;

namespace WarehouseManagement.API.Application.Authentication;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(IConfiguration config, IUnitOfWork unitOfWork)
    {
        _config = config;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var userModel = await AuthenticateUser(command);
        if (userModel is null)
        {
            return Unauthorized("Invalid credentials");
        }

        if (!userModel.Active)
        {
            return BadRequest("This account is disabled, please contact support");
        }

        var token = GenerateJwtToken(userModel);
        return Ok(new { token });
    }

    private string GenerateJwtToken(UserModel model)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, model.Username),
            new Claim("role", model.Role),
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private async Task<UserModel?> AuthenticateUser(LoginCommand command)
    {
        var user = await _unitOfWork.Users.SingleOrDefaultAsync(new UserByNameSpec(command.Username));
        if (user != null && VerifyPassword(user, command.Password) && user.Active)
        {
            return new UserModel { Username = user.UserName, Role = user.Role.ToString(), Active = user.Active };
        }

        return null;
    }

    public bool VerifyPassword(User user, string providedPassword)
    {
        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.Password, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}

public class UserModel
{
    public string Username { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool Active { get; set; }
}
