using System.Text.Json;
using TreeApp.Domain.Entities;
using TreeApp.Domain.Exceptions;
using TreeApp.Domain.Interfaces;

namespace TreeApp.Api.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    IServiceProvider serviceProvider,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var eventId = DateTime.UtcNow.Ticks;
        var requestBody = await ReadRequestBody(context.Request);
        var queryString = context.Request.QueryString.ToString();
        
        using var scope = serviceProvider.CreateScope();
        var journalRepo = scope.ServiceProvider.GetRequiredService<IJournalRepository>();

        var entry = new JournalEntry
        {
            EventId = eventId,
            CreatedAt = DateTime.UtcNow,
            QueryString = queryString,
            RequestBody = requestBody,
            StackTrace = exception.StackTrace,
            ExceptionType = exception.GetType().Name,
            ExceptionMessage = exception.Message
        };

        await journalRepo.AddAsync(entry);
        await journalRepo.SaveChangesAsync();
        
        var response = exception switch
        {
            SecureException secureEx => new
            {
                type = secureEx.GetType().Name.Replace("Exception", ""),
                id = eventId.ToString(),
                data = new { message = secureEx.Message }
            },
            _ => new
            {
                type = "Exception",
                id = eventId.ToString(),
                data = new { message = $"Internal server error ID = {eventId}" }
            }
        };

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(response, 
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }

    private static async Task<string> ReadRequestBody(HttpRequest request)
    {
        if (request.ContentLength == 0 || request.Body.Length == 0)
            return string.Empty;

        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }
}