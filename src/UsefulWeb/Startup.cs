// <copyright file="Startup.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulWeb
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.Interfaces;
    using Useful.UI.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<ICipherRepository, CipherRepository>();
            services.AddTransient<CipherService, CipherService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}