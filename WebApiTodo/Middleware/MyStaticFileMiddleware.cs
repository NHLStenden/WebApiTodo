using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WebApiTodo.Middleware
{
    public static class MyStaticFileMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyStaticFileMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyStaticFileMiddleware>();
        }
    }
    
    public class MyStaticFileMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MyStaticFileMiddleware(RequestDelegate next, IWebHostEnvironment webHostEnvironment)
        {
            _next = next;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value;
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }
                
            var staticFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", path);
                
            if (File.Exists(staticFilePath))
            {
                var extension = new FileInfo(staticFilePath).Extension;

                //https://en.wikipedia.org/wiki/Media_type
                string mediaType = extension switch
                {
                    ".jpeg" => "image/jpeg",
                    ".jpg" => "image/jpeg",
                    ".html" => "text/html",
                    _ => throw new NotImplementedException($"mediaType for extension: {extension} not supported")
                };
                context.Response.Headers.Add("content-type", mediaType);

                await using (var stream = File.OpenRead(staticFilePath))
                {
                    await stream.CopyToAsync(context.Response.Body);    
                }
                    
                await context.Response.CompleteAsync();
                // context.Response.Body.WriteAsync()
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}