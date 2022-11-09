using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Tutorial9.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            }
            catch (Exception exc)
            {
                string filePath = "logs.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {

                    while (exc != null)
                    {
                        writer.WriteLine(exc.GetType().FullName);
                        writer.WriteLine("Message : " + exc.Message);
                        writer.WriteLine("StackTrace : " + exc.StackTrace);

                        exc = exc.InnerException;
                    }
                }
            }
        }
    }
}
