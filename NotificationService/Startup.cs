using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace NotificationService
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        private SwaggerAPIOptions swaggerAPIOptions;

        //get swagger config
        private SwaggerAPIOptions SwaggerAPIOptions
        {
            get
            {//if not availble create else retunred cached
                if (swaggerAPIOptions is null)
                {
                    swaggerAPIOptions = Configuration.GetSection("SwaggerAPIOptions").Get<SwaggerAPIOptions>();
                }
                return swaggerAPIOptions;

            }
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var token = Configuration["Settings:authenticationToken"];
            var telegramBotClient = new TelegramBotClient(token);

            UpdateType[] allowedUpdates = { UpdateType.Message };

            services.AddScoped<ITelegramBotClient>(client => telegramBotClient);

            if (SwaggerAPIOptions.Enabled)
                services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //if enabled 
            if (SwaggerAPIOptions.Enabled)
            {

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DocumentTitle = SwaggerAPIOptions.DocumentTitle;
                    c.SwaggerEndpoint(SwaggerAPIOptions.Path, SwaggerAPIOptions.VersionName);
                    c.RoutePrefix = SwaggerAPIOptions.RoutePrefix;
                    c.DisplayRequestDuration();


                });
            }
            app.UseAuthorization();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
