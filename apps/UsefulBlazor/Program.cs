// Copyright (c) Andrew Hawkins. All rights reserved.

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

[assembly: CLSCompliant(false)]

namespace UsefulBlazor;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebAssemblyHostBuilder? builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        await builder.Build().RunAsync().ConfigureAwait(false);
    }
}
