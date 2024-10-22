using HappyCompany.AspNetCore.Http;
using HappyCompany.AspNetCore.Http.ExceptionHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HappyCompany.AspNetCore.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IHostApplicationBuilder AddDefaultExceptionHandler(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.Configure<ExceptionHandlingProviderOptions>(AddDefaultExceptionHandlers);
        services.AddSingleton<ExceptionHandlingProvider>();

        return builder;
    }

    private static void AddDefaultExceptionHandlers(ExceptionHandlingProviderOptions options)
    {
        options.AddExceptionHandler(new CannotInsertNullExceptionHandler());
        options.AddExceptionHandler(new UniqueConstraintExceptionHandler());
    }
}