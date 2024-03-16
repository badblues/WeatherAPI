using System.Configuration;
using Serilog;
using WebAPI.Middleware;
using WebAPI.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);

string? defaultApiKey = builder.Configuration.GetSection("DefaultApiKey").Value;
string? cityLocationApiUrl = builder.Configuration.GetSection("CityLocationApiUrl").Value;
string? weatherApiUrl = builder.Configuration.GetSection("WeatherApiUrl").Value;

if (cityLocationApiUrl == null || weatherApiUrl == null)
{
    throw new ConfigurationErrorsException("API URLs aren't configured");
}

if (defaultApiKey == null)
{
    throw new ConfigurationErrorsException("Default ApiKey isn't configured");
}

builder.Services.AddControllers()
    .AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddTransient(s =>
        {
            HttpClient? client = s.GetService<HttpClient>();
            return new CityLocationApiService(client!, cityLocationApiUrl, defaultApiKey);
        }
    );

builder.Services.AddTransient(s =>
        {
            HttpClient? client = s.GetService<HttpClient>();
            return new WeatherApiService(client!, weatherApiUrl, defaultApiKey);
        }
    );

builder.Services.AddTransient<WeatherDataService>();

builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

WebApplication app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
