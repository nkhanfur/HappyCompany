using Microsoft.Extensions.Options;

namespace HappyCompany.AspNetCore.Http;

public class ExceptionHandlingProvider
{
    private readonly ExceptionHandlingProviderOptions _options;

    public ExceptionHandlingProvider(IOptions<ExceptionHandlingProviderOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Get the exception handler for the given exception type.
    /// </summary>
    /// <param name="exception">Exception.</param>
    /// <returns>Exception handler of type <see cref="IExceptionHandler"/> for the given exception type. if no handler was found, <see langword="null"/> will be returned.</returns>
    internal IExceptionHandler? GetExceptionHandler(Exception exception)
    {
        _options.ExceptionHandlers.TryGetValue(exception.GetType(), out var exceptionHandler);
        return exceptionHandler;
    }
}
