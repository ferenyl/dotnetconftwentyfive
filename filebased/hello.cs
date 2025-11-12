#:package Spectre.Console@0.53.0

using Spectre.Console;

var message = args.FirstOrDefault() ?? "World";

AnsiConsole.Write(new FigletText($"Hello {message}").LeftJustified().Color(Color.Red));
