// Copyright (c) Andrew Hawkins. All rights reserved.

using Microsoft.Extensions.Logging;
using Useful.Audio.Midi;

[assembly: CLSCompliant(false)]

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddSimpleConsole(options => options.SingleLine = true));
ILogger logger = factory.CreateLogger<Program>();

using FileStream file = File.OpenRead(Path.Combine("..", "..", "..", "..", "..", "test", "Useful.Audio.Tests", "danube.mid"));

MidiFile midiFile = new(file, logger);
