namespace ToDo.WebApi.Middleware
{
    /// <summary>
    /// Middleware to handle global errors
    /// </summary>
    public class GlobalErrorHandling
    {
        private RequestDelegate _next;
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalErrorHandling"/> class.
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/></param>
        public GlobalErrorHandling(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/></param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                switch (ex)
                {
                    case UnauthorizedAccessException:
                        response.StatusCode = (int)StatusCodes.Status401Unauthorized;
                        break;
                    case System.FormatException:
                        response.StatusCode = (int)StatusCodes.Status401Unauthorized;
                        break;
                    default:
                        response.StatusCode = (int)StatusCodes.Status500InternalServerError;
                        break;
                }
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
        }
    }
}