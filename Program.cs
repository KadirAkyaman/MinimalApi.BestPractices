using System.Reflection;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using MinimalApi.BestPractices.DTOs;
using MinimalApi.BestPractices.ValidationFilters;
using MinimalApi.BestPractices.Validators;
using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV" ;
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("post-user-policy", httpContext =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown_ip",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromSeconds(10)
            });
    });
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken: token);        
    };
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api Gateway Nucleus",
        Version = "v1"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var versionSet = app.NewApiVersionSet().HasApiVersion(new ApiVersion(1, 0)).Build();


app.MapPost("/api/v{version:apiVersion}/users", (CreateUserDto createUserDto) =>
{
    return Results.Ok("User created successfully."); // Başarı mesajını da ekleyelim
})
.WithApiVersionSet(versionSet)
.MapToApiVersion(1, 0)
.RequireRateLimiting("post-user-policy")
.AddEndpointFilter<ValidationFilter<CreateUserDto>>()
.WithOpenApi(operation =>
{
    operation.Summary = "Creates a new user in the system.";
    operation.Description = "This endpoint validates the incoming user data based on predefined rules and is protected by a rate limiter.";
    
    operation.Responses["200"].Description = "Returns a success message if the user was created successfully.";
    operation.Responses.Add("400", new() { Description = "Returned if the provided user data is invalid (e.g., weak password, invalid email)." });
    operation.Responses.Add("429", new() { Description = "Returned if the client has exceeded the rate limit." });

    return operation;
});


app.MapGet("/api/v{version:apiVersion}/data/{source:alpha:length(3,10)}", ([FromRoute] string source, [AsParameters] DataFilterDto filter) =>
{
    var res = new { Source = source, Filter = filter };
    return Results.Ok(res);
})
.WithApiVersionSet(versionSet)
.MapToApiVersion(1, 0)
.WithOpenApi(operation =>
{
    operation.Summary = "Retrieves data from a specified source, with optional filtering.";
    operation.Description = "The data source is specified in the URL path and must conform to the defined constraints.";

    // Parametreleri detaylandır
    var sourceParam = operation.Parameters.First(p => p.Name == "source");
    sourceParam.Description = "The name of the data source. Must be 3 to 10 alphabetic characters.";
    sourceParam.Example = new Microsoft.OpenApi.Any.OpenApiString("sensor");

    foreach (var param in operation.Parameters)
    {
        if (param.Name == "Tags")
        {
            param.Description = "A collection of tags to filter the data by. Can be provided multiple times.";
            param.Example = new Microsoft.OpenApi.Any.OpenApiArray
            {
                new Microsoft.OpenApi.Any.OpenApiString("temp"),
                new Microsoft.OpenApi.Any.OpenApiString("humidity")
            };
        }
        else if (param.Name == "StartDate")
        {
            param.Description = "The starting date to filter the data from.";
            param.Example = new Microsoft.OpenApi.Any.OpenApiString("2025-10-28");
        }
        else if (param.Name == "SortBy")
        {
            param.Description = "The field to sort the data by.";
            param.Example = new Microsoft.OpenApi.Any.OpenApiString("timestamp");
        }
    }

    // Olası yanıtları tanımla
    operation.Responses["200"].Description = "Returns a summary of the requested data and applied filters.";
    operation.Responses.Add("404", new() { Description = "Returned if the source name does not meet the route constraints." });
    
    return operation;
});

app.Run();
