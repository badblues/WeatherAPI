using System.Configuration;
using Serilog;
using WebAPI.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.json", optional: false);

string? defaultApiKey = builder.Configuration.GetSection("DefaultApiKey").Value;

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
            return new CityLocationApiService(client!, defaultApiKey);
        }
    );

builder.Services.AddTransient(s =>
        {
            HttpClient? client = s.GetService<HttpClient>();
            return new WeatherApiService(client!, defaultApiKey);
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
