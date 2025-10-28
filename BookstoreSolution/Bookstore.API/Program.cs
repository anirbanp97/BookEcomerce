using Bookstore.API.DependencyInjection;
using Bookstore.API.Middleware;
using Bookstore.API.SerilogConfig;
using Bookstore.Common;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
SerilogConfiguration.ConfigureSerilog(builder);

// Add Controllers & Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddRepositories(builder.Configuration)
                .AddServices()
                .AddLoggingServices(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy.WithOrigins("https://localhost:7137") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Build
var app = builder.Build();

// Middleware
app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Logger.Information("Bookstore API started successfully.");
app.Run();