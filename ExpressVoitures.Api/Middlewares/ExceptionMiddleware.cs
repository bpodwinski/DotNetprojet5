using Newtonsoft.Json;

public class ApiException : Exception
{
    public int StatusCode { get; }
    public override string Message { get; }

    public ApiException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}

public class EmailExistsException : ApiException
{
    public EmailExistsException()
        : base(StatusCodes.Status409Conflict, "Email already exists")
    {
    }
}

public class InvalidParameterException : ApiException
{
    public InvalidParameterException(string parameterName)
        : base(StatusCodes.Status400BadRequest, $"Invalid parameter: {parameterName}.")
    {
    }
}

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (exception is ApiException apiException)
        {
            context.Response.StatusCode = apiException.StatusCode;
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new { error = apiException.Message });
            return context.Response.WriteAsync(result);
        }

        // Gérer les autres types d'exceptions non spécifiques
        _logger.LogError(exception, "Unhandled exception.");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        var defaultResult = JsonConvert.SerializeObject(new { error = "An unexpected error occurred. Please try again later." });
        return context.Response.WriteAsync(defaultResult);
    }
}