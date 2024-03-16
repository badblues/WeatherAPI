#### Run
Make sure to configure DefaultApiKey in appsettings.json

###### Unix:
From solution root directory:  
`docker build -t webapi -f WebAPI/Dockerfile .`  
`docker run -p 8080:80  -v "$(pwd)/WebAPI/logs:/app/logs" webapi`

#### Endpoints

- GET /weather/{city}?apiKey={apiKey} : Weather for specific city
  - Parameters:
    - 'city': Name of the city, must be spelled correctly
    - 'apiKey' (optional): [OpenWeatherMap](https://openweathermap.org/) API key with 3.0 API access
  - Examples:
    - 'weather/London?apiKey=your_api_key'
    - 'weather/London'
- GET /weather/{firstCity}/{secondCity}?apiKey={apiKey}: Average weather between two cities
  - Parameters:
    - firstCity: Name of the first city.
    - secondCity: Name of the second city.
    - apiKey (optional): OpenWeatherMap API key with 3.0 API access. 
  - Examples:
    - /weather/London/Paris?apiKey=your_api_key
    - /weather/London/Paris
- GET /weather/{city}/xml?apiKey={apiKey}: Weather for specific city in XML format
  - Parameters:
    - city: Name of the city, must be spelled correctly.
    - apiKey (optional): OpenWeatherMap API key with 3.0 API access.
  - Example:
    - /weather/London/xml?apiKey=your_api_key
    - /weather/London/xml
- GET /weather/{firstCity}/{secondCity}/xml?apiKey={apiKey}: Average weather between two cities in XML format
  - Parameters:
    - firstCity: Name of the first city.
    - secondCity: Name of the second city.
    - apiKey (optional): OpenWeatherMap API key with 3.0 API access.
  - Example:
    - /weather/London/Paris/xml?apiKey=your_api_key
    - /weather/London/Paris/xml
