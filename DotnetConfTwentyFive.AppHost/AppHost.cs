var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.FirstProject>("firstproject");

builder.Build().Run();
