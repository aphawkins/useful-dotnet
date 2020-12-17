// <copyright file="WeatherForecastService.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulBlazor.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class WeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            Random? rng = new();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
            }).ToArray());
        }
    }
}