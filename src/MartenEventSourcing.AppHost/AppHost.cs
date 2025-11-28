var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("marten-db")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin(config =>
        config.WithLifetime(ContainerLifetime.Persistent));

builder.AddProject<Projects.MartenEventSourcing_Web>("web-api")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
