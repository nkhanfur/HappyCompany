using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace HappyCompany.AspNetCore.Http.ExceptionHandlers;

public partial class CannotInsertNullExceptionHandler : ExceptionHandler<CannotInsertNullException>
{
    protected override ProblemDetails HandleException(HttpContext context, CannotInsertNullException exception)
    {
        var regex = ColumnRegex();
        var matches = regex.Matches(exception.InnerException!.Message);
        var column = string.Empty;

        if (matches.Count > 0 && matches.First().Groups.ContainsKey("column"))
        {
            column = matches.First().Groups["column"].Value;
        }

        return new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Required Field",
            Detail = $"Field '{column}' can't be null.",
        };
    }

    [GeneratedRegex("'(?<column>.*)',")]
    private static partial Regex ColumnRegex();
}