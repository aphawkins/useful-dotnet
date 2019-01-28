// <copyright file="Program.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulWeb
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
#pragma warning disable IDISP004 // Don't ignore return value of type IDisposable.
            CreateWebHostBuilder(args).Build().Run();
#pragma warning restore IDISP004 // Don't ignore return value of type IDisposable.
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
