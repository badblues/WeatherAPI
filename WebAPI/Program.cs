using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);

string defaultApiKey = builder.Configuration.GetSection("DefaultApiKey").Value;

builder.Services.AddControllers()
    .AddXmlSerializerFormatters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddTransient( s =>
        {
            HttpClient client = s.GetService<HttpClient>();
            return new CityLocationApiService(client, defaultApiKey);
        }
    );

builder.Services.AddTransient(s =>
        {
            HttpClient client = s.GetService<HttpClient>();
            return new WeatherInfoApiService(client, defaultApiKey);
        }
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
