using Domain.Realtime;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using OrdenesCompra.Extensions;
using Persistence;
using Persistence.Repositories;
using Realtime.Hubs;
using Realtime.Services;
using Serilog;
using Service.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Service extensions

builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

builder.Host.UseSerilog((hostContext, configuration) => {
    configuration.ReadFrom.Configuration(hostContext.Configuration);
});

builder.Services.AddDbContext<RepositoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

// Realtime

builder.Services.AddSignalR();
builder.Services.AddScoped<IRealtimeManager, RealtimeManager>();

// Middlewares

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.MapHub<CodeHub>("/hubs/code");

app.Run();
