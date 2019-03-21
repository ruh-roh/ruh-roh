﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RuhRoh.Extensions.Microsoft.DependencyInjection;
using RuhRoh.Samples.WebAPI.Data;
using RuhRoh.Samples.WebAPI.Data.Services;
using RuhRoh.Samples.WebAPI.Domain;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("TodoDb")));
            services.AddScoped<ITodoItemService, TodoItemService>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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

            Func<Task<TodoItem>, Task<TodoItem>> taskWithChaosFunc = async task =>
            {
                var item = await task;
                item.Completed = !item.Completed;
                item.Description = string.Concat("CHAOS - ", item.Description, " - CHAOS");

                return item;
            };

            services.AffectScoped<ITodoItemService, TodoItemService>()
                .WhenCalling(x => x.GetTodoItem(With.Any<Guid>()))
                .ReturnsAsync<TodoItem>(x => taskWithChaosFunc(x))
                .EveryNCalls(2);

            services.AffectScoped<ITodoItemService, TodoItemService>()
                .WhenCalling(x => x.GetTodoItem(With.Any<Guid>()))
                .Throw<Exception>()
                .EveryNCalls(3)
                .And
                // During 5 minutes after running, every third call should fail
                .PlannedAt(new DateTimeOffset(DateTime.Now.AddMinutes(-40)), TimeSpan.FromMinutes(45));
        }
        
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
