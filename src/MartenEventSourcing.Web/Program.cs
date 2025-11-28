using JasperFx.Events.Daemon;
using JasperFx.Events.Projections;
using Marten;
using MartenEventSourcing.Web.Tickets.Projections;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddNpgsqlDataSource("marten-db");

// Register Marten
builder.Services.AddMarten(options =>
    {
        // Optional, defaults to "public"
        options.DatabaseSchemaName = "martendemo";

        options.Projections
            .Add<OpenTicketProjection>(ProjectionLifecycle.Async);
    })
    // Use Aspire Data Source for connection string
    .UseNpgsqlDataSource()
    // Pick a default session type
    .UseLightweightSessions()
    // Start an asynchronous projection daemon
    .AddAsyncDaemon(DaemonMode.Solo);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/swagger/v1/swagger.json");
    app.UseSwaggerUI();

    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
