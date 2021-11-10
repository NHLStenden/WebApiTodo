using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebApiTodo.Models;
using WebApiTodo.Repositories;

namespace WebApiTodo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddXmlDataContractSerializerFormatters();
            
            
            
            services.AddDbContext<TodoContext>(builder =>
            {
                builder.UseMySQL("Server=localhost;Database=WebApiTodo;Uid=root;Pwd=Test@1234!;");
            });

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITodoRepository, TodoRepository>();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiTodo", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TodoContext db)
        {
            app.Use(async (context, next) =>
            {
                string path = context.Request.Path.Value;
                if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }
                
                var staticFilePath = Path.Combine(env.ContentRootPath, "wwwroot", path);
                
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
                    await next.Invoke();
                }
            });
            
            if (env.IsDevelopment())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                DatabaseSeeder.Seed(db);
                
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiTodo v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

    public static class DatabaseSeeder
    {
        public static void Seed(TodoContext db)
        {
            foreach (var i in Enumerable.Range(1, 10))
            {
                var category = new Category()
                {
                    Name = $"Category {i}"
                };
                //db.Categories.Add(category);

                var now = DateTime.Now;
                foreach (var todoIndex in Enumerable.Range(1,5))
                {
                    var todo = new Todo()
                    {
                        Description = $"Todo {todoIndex}, Category: {category.Name}",
                        Completed = todoIndex % 2 == 0,
                        Category = category,
                        DueDate = now
                    };

                    db.Todos.Add(todo);
                    now = now.AddDays(1);
                }
            }

            db.SaveChanges();
        }
    }
}