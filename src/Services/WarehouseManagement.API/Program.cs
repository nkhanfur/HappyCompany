using FluentValidation;
using FluentValidation.AspNetCore;
using HappyCompany.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WarehouseManagement.API.Application.Extensions;
using WarehouseManagement.API.Application.Swagger;
using WarehouseManagement.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add infrastructure
builder.AddInfrastructure();

// Add HTTP context accessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://happycompany.com",
        ValidAudience = "https://happycompany.com",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKey"))
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
    .AddPolicy("Management", policy => policy.RequireRole("Management"))
    .AddPolicy("Auditor", policy => policy.RequireRole("Auditor")); ;

// Add Serilog Logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder =>
    loggingBuilder.AddSerilog(dispose: true));

builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddSmartMapper(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        Name = "JWT Auth",
        Description = "Enter JWT Bearer token **_only_**",
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Scheme = "bearer", // must be lower case for token header
        BearerFormat = "JWT"
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.AddDefaultApiVersioning();

builder.AddDefaultExceptionHandler();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: add CORS policy
app.UseCors(options => options
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true));

// Configure security
app.UseReferrerPolicy(options => options.NoReferrer());
app.UseCsp(options => options
    .DefaultSources(s => s.Self())
    .ReportUris(r => r.Uris("/report"))
    .UpgradeInsecureRequests());
app.UseHsts(options => options
    .MaxAge(days: 30)
    .IncludeSubdomains());
// Must be set after app.UseStaticFiles()
app.UseNoCacheHttpHeaders();
app.UseRedirectValidation(options => options.AllowSameHostRedirectsToHttps());  // Same host redirects should be set when UpgradeInsecureRequests is used
app.UseXfo(options => options.SameOrigin());
app.UseXXssProtection(options => options.EnabledWithBlockMode());
app.UseXContentTypeOptions();
app.UseXDownloadOptions();
app.UseXRobotsTag(options => options.NoIndex().NoFollow());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
