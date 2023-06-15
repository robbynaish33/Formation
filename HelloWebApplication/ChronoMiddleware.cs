using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HelloWebApplication
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ChronoMiddleware
    {
        private readonly RequestDelegate _next;

        public ChronoMiddleware(
            RequestDelegate next,
            IConfiguration conf,
            ILogger<ChronoMiddleware> logger)
        {
            _next = next;
            string cp = conf.GetValue<string>("Copyright");
            logger.LogInformation("© " + cp);

            //string path = conf.GetValue<string>("PATH");
            //logger.LogInformation("PATH " + path);
        }

        public async Task InvokeAsync(
            HttpContext httpContext,
            ILogger<ChronoMiddleware> logger,
            IConfiguration conf,
            IMagicService magic
            )
        {
            Stopwatch chrono = Stopwatch.StartNew();
            logger.LogInformation("MAGIC NUMBER => " + magic.MagicNumber);
            await _next(httpContext);
            chrono.Stop();
            long tempsExecution = chrono.ElapsedMilliseconds;
            string message = $"la requête [{httpContext.Request.Path}] a été exécutée en {tempsExecution}ms";

            var confCrono = conf.GetSection("Chrono");
            var si = confCrono.GetValue<int>("Seuils:Info");
            var sw = confCrono.GetValue<int>("Seuils:Warn");
            var se = confCrono.GetValue<int>("Seuils:Err");

            /*if (tempsExecution <= si)
                logger.LogInformation(message);
            else if (tempsExecution <= sw)
                logger.LogWarning(message);
            else if (tempsExecution <= se)
                logger.LogError(message);
            else
                logger.LogCritical(message);*/
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ChronoMiddlewareExtensions
    {
        public static IApplicationBuilder UseChronoMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ChronoMiddleware>();
        }
    }
}
