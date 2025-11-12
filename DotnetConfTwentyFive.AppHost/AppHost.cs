var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.FirstProject>("firstproject");

builder.AddProject<Projects.WhatsNew>("whatsnew");

builder.AddProject<Projects.McpServer>("mcpserver");

builder.Build().Run();
