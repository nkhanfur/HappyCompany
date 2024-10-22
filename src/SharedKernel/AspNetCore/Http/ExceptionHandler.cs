using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyCompany.AspNetCore.Http;

public abstract class ExceptionHandler<TException> : IExceptionHandler where TException : Exception
{
    public virtual ProblemDetails Handle(HttpContext context)
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>()!;

        // Cast the exception explicitly.
        // Exception will be always the same type since this is handled by the exception handling provider.
        var exception = (TException)exceptionFeature.Error;

        return HandleException(context, exception);
    }

    protected abstract ProblemDetails HandleException(HttpContext context, TException exception);
}
