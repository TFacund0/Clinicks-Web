using System.Collections.Generic;

namespace ClinicksApi.Business.Interfaces
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetForecasts();
    }
}
