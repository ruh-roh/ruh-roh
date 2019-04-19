using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RuhRoh.Extensions.Microsoft.DependencyInjection;
using RuhRoh.Samples.WebAPI.Data;
using RuhRoh.Samples.WebAPI.Data.Services;
using RuhRoh.Samples.WebAPI.Domain.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace RuhRoh.Samples.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("TodoDb")));
            services.AddScoped<ITodoItemService, TodoItemService>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "ASP.NET Core Web API",
                    Version = "v1"
                });
            });
            
            ConfigureRuhRoh(services);
        }

        private void ConfigureRuhRoh(IServiceCollection services)
        {
            services.AffectScoped<ITodoItemService, TodoItemService>()
                .WhenCalling(x => x.GetAllTodoItems())
                .SlowItDownBy(TimeSpan.FromSeconds(10))
                .AtRandom();

            services.AffectScoped<ITodoItemService, TodoItemService>()
                .WhenCalling(x => x.AddNewTodoItem(With.Any<string>()))
                .Throw<Exception>()
                .AtRandom();

            services.AffectScoped<ITodoItemService, TodoItemService>()
                .WhenCalling(x => x.GetTodoItem(With.Any<Guid>()))
                .Throw<Exception>()
                .EveryNCalls(3)
                .And
                // During 5 minutes after running, every third call should fail
                .PlannedAt(new DateTimeOffset(DateTime.Now.AddMinutes(-40)), TimeSpan.FromMinutes(45));
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, TodoDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDatabaseErrorPage();
                app.UseDeveloperExceptionPage();
                // Redirect to /swagger
                var option = new RewriteOptions();
                option.AddRedirect("^$", "swagger");
                app.UseRewriter(option);
            }

            context.Database.Migrate();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET Core Web API v1");
            });

            app.UseMvc();
        }
    }
}
