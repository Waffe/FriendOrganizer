using System;
using System.Threading.Tasks;
using FriendOrganizer.UI.View.Services.WeatherModels;

namespace FriendOrganizer.UI.View.Services
{
    public interface IWeatherService
    {
        Task<ConsolidatedWeather> GetLocationWeatherForDateAsync(DateTime date, string location);
    }
}