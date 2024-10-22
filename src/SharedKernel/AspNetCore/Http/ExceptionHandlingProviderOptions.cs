namespace HappyCompany.AspNetCore.Http;

public class ExceptionHandlingProviderOptions
{
    public bool EnableDefaultHandlers { get; set; } = true;

    internal IDictionary<Type, IExceptionHandler> ExceptionHandlers { get; } = new Dictionary<Type, IExceptionHandler>();

    public void AddExceptionHandler<TException>(ExceptionHandler<TException> handler) where TException : Exception
    {
        ExceptionHandlers.Add(typeof(TException), handler);
    }
}