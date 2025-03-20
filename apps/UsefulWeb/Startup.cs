// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful;
using Useful.Security.Cryptography;
using Useful.Security.Cryptography.UI.Services;

namespace UsefulWeb;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    public static void ConfigureServices(IServiceCollection services)
    {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        services.Configure<CookiePolicyOptions>(options => options.CheckConsentNeeded = _ => true);

        services.AddTransient<IRepository<ICipher>, WebCipherRepository>();
        services.AddTransient<CipherService, CipherService>();

        services.AddControllersWithViews();
        services.AddRazorPages();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseCookiePolicy();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                name: "cryptography",
                pattern: "{controller=Cryptography}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }
}
