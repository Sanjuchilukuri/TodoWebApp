using System.Diagnostics;
using Serilog;

namespace ToDo.WebApi.Middleware
{
    /// <summary>
    /// Middleware for logging HTTP requests
    /// </summary>
    public class Logging
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logging"/> class.
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/></param>
        public Logging(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware
        /// </summary>
        /// <param name="context"><see cref="HttpContext"></param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="Exception">Throws the exception middleware.</exception>
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);
                stopwatch.Stop();
                var logTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

                Log.Information(logTemplate,
                                context.Request.Method,
                                context.Request.Path,
                                context.Response.StatusCode,
                                stopwatch.Elapsed.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                var logTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} Error \n{errorMessage}\n";

                Log.Information(logTemplate,
                                context.Request.Method,
                                context.Request.Path,
                                context.Response.StatusCode,
                                ex);

                throw;
            }
        }
    }
}