using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RuhRoh.Samples.WebAPI.Data;
using RuhRoh.Samples.WebAPI.Data.Services;
using RuhRoh.Samples.WebAPI.Domain.Services;

namespace RuhRoh.Samples.WebAPI
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
            services.AddDbContext<TodoDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("TodoDb")));
            services.AddScoped<ITodoItemService, TodoItemService>();

            services.AddMvc();

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
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDatabaseErrorPage();
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
