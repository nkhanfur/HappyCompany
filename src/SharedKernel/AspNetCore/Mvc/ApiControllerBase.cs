using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mime;

namespace HappyCompany.AspNetCore.Mvc;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("v{version:apiVersion}/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected record struct Pagination(int PageSize);

    protected static readonly Pagination PaginationOptions = new(10);

    protected ObjectResult ValidationBadRequest(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        var problemDetails = new ValidationProblemDetails(ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Extensions =
            {
                ["traceId"] = Activity.Current?.Id
            }
        };

        return new BadRequestObjectResult(problemDetails);
    }

    protected ObjectResult BadRequest(string title, string? detail = null)
    {
        Activity.Current?.SetTag("http.response_problem_title", title);

        return Problem(title: title, statusCode: StatusCodes.Status400BadRequest, detail: detail);
    }

    protected static PaginationResult? CreatePagination(int? pageNumber = null, int? pageSize = null, int? pageCount = null, int? totalItemCount = null, bool? firstPage = null, bool? lastPage = null)
    {
        if (pageNumber is null && pageSize is null)
        {
            return default;
        }

        pageSize ??= PaginationOptions.PageSize;

        return new PaginationResult()
        {
            PageSize = pageSize.Value,
            PageCount = pageCount ?? 0,
            PageNumber = pageNumber ?? 1,
            LastPage = lastPage ?? false,
            FirstPage = firstPage ?? true,
            TotalItemCount = totalItemCount ?? 0,
        };
    }
}