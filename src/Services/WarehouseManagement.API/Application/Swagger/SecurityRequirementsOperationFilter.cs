using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WarehouseManagement.API.Application.Swagger;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Policy names map to scopes
        var requiredAuths = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Select(attr => (attr.AuthenticationSchemes, attr.Policy))
            .Distinct();

        if (requiredAuths.Any())
        {
            var scopes = requiredAuths.Select(r => r.Policy).ToList();
            var authSchemes = requiredAuths.Select(r => r.AuthenticationSchemes)
                .SelectMany(s => s?.Split(',') ?? []).ToList();
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            var securityRequirements = new List<OpenApiSecurityRequirement>();
            if (authSchemes.Contains(JwtBearerDefaults.AuthenticationScheme) || authSchemes.Count == 0)
            {
                var jwtBearerScheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme } };
                securityRequirements.Add(new OpenApiSecurityRequirement { [jwtBearerScheme] = scopes });
            }

            operation.Security = securityRequirements;
        }
    }
}
