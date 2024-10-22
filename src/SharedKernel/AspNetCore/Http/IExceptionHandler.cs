using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyCompany.AspNetCore.Http;

internal interface IExceptionHandler
{
    ProblemDetails Handle(HttpContext context);
}