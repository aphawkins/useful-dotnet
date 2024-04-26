// Copyright (c) Andrew Hawkins. All rights reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Useful.Audio.Midi;

[assembly: CLSCompliant(false)]

IConfigurationRoot configBuilder = new ConfigurationBuilder().Build();

using ILoggerFactory factory = LoggerFactory.Create(builder
    => builder
    .AddConfiguration(configBuilder.GetSection("Logging"))
    .AddSimpleConsole(options => options.SingleLine = true)
    .AddDebug());
ILogger logger = factory.CreateLogger<Program>();

using FileStream file = File.OpenRead(Path.Combine("..", "..", "..", "..", "..", "test", "Useful.Audio.Tests", "theme.mid"));

MidiFile midiFile = new(file, logger);
