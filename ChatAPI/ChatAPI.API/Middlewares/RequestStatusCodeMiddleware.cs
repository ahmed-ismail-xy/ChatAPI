namespace ChatAPI.API.Middlewares
{
    public class RequestStatusCodeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestStatusCodeMiddleware> _logger;

        public RequestStatusCodeMiddleware(ILogger<RequestStatusCodeMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            // Generate a request ID.
            string requestId = Guid.NewGuid().ToString();

            // Add the request ID to the response headers.
            httpContext.Response.Headers.Add("Request-Id", requestId);

            // Log the request information.
            _logger.LogInformation("Start Request : Request ID: {requestId} Request: {request}", requestId, httpContext.Request.Path);

            // Log the status code.

            // Call the next middleware in the pipeline.
            await next(httpContext);

            // Log the response status code.
            _logger.LogInformation("End Request : Request ID: {requestId} Status Code: {statusCode}", httpContext.Response.Headers["Request-Id"], httpContext.Response.StatusCode);

        }
    }

}
