using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyCompany.AspNetCore.Extensions;

public static class ApiVersioningExtensions
{
    public static IHostApplicationBuilder AddDefaultApiVersioning(this IHostApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        }).AddMvc(options =>
        {
            options.Conventions.Add(new VersionByNamespaceConvention());
        });

        return builder;
    }
}