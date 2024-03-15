using Serilog;
using System.Configuration;
using WebAPI.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Configuration.AddJsonFile("appsettings.json", optional: false);

string? defaultApiKey = builder.Configuration.GetSection("DefaultApiKey").Value;

if (defaultApiKey == null)
    throw new ConfigurationErrorsException("DefaultApiKey isn't configured");

builder.Services.AddControllers()
    .AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddTransient(s =>
        {
            HttpClient? client = s.GetService<HttpClient>();
            return new CityLocationApiService(client!, defaultApiKey);
        }
    );

builder.Services.AddTransient(s =>
        {
            HttpClient? client = s.GetService<HttpClient>();
            return new WeatherApiService(client!, defaultApiKey);
        }
    );

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

WebApplication app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
