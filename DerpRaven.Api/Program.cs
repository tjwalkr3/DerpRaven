using DerpRaven.Api;
using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddLogging();

// Add authentication services
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Authority = "https://engineering.snow.edu/auth/realms/SnowCollege";
    options.Audience = "JonathanMauiAuth";
});

// Add the database context
string dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new ArgumentNullException("Database connection string is missing!");
builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseNpgsql(dbConnectionString);
});

// Get exporter URL
Uri otlpEndpoint = new Uri(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://localhost:4318");

var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService("DerpRaven");

// Set up OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .SetResourceBuilder(resourceBuilder)
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otlpEndpoint, "/v1/traces");
            options.Protocol = OtlpExportProtocol.HttpProtobuf;
        }))
    .WithMetrics(metrics => metrics
        .SetResourceBuilder(resourceBuilder)
        .AddAspNetCoreInstrumentation()
        .AddMeter("DerpRaven")
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otlpEndpoint, "/v1/metrics");
            options.Protocol = OtlpExportProtocol.HttpProtobuf;
        }))
    .WithLogging(logging => logging
        .SetResourceBuilder(resourceBuilder)
        .AddConsoleExporter()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otlpEndpoint, "/v1/logs");
            options.Protocol = OtlpExportProtocol.HttpProtobuf;
        }));

builder.Services.AddOpenApi();
builder.Services.Configure<BlobStorageOptions>(builder.Configuration.GetSection("BlobStorage"));
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICustomRequestService, CustomRequestService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlobService, BlobService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
