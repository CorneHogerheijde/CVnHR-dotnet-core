using System.Collections.Generic;
using CVnHR.manage.UI.Models;

namespace CVnHR.manage.UI.Providers
{
    public interface IWeatherProvider
    {
        List<WeatherForecast> GetForecasts();
    }
}
