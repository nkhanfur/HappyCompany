using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyCompany.AspNetCore.Http.ExceptionHandlers;

public class UniqueConstraintExceptionHandler : ExceptionHandler<UniqueConstraintException>
{
    protected override ProblemDetails HandleException(HttpContext context, UniqueConstraintException exception)
    {
        return new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Duplicated Field Value",
            Detail = "Some fields have duplicated values."
        };
    }
}