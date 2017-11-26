using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FriendOrganizer.UI.View.Services.WeatherModels;
using Newtonsoft.Json;

namespace FriendOrganizer.UI.View.Services
{
    public class WeatherService : IWeatherService
    {
        private static HttpClient _httpClient;
        private const string DemoPurposeWoeid = "890869";

        public WeatherService()
        {
            _httpClient = new HttpClient {BaseAddress = new Uri("https://www.metaweather.com/api/")};
        }

        internal async Task<string> GetWoeid(string location)
        {
            if (CheckForInternetConnection())
            {
                var response = await _httpClient.GetAsync($"location/search/?query={location}");
                var responseString = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<ApiLocation[]>(responseString);

                return model[0].woeid.ToString();
            }
            return null;
        }



        public class ApiLocation
        {
            public string title { get; set; }
            public string location_type { get; set; }
            public int woeid { get; set; }
            public string latt_long { get; set; }
        }


        public async Task<ConsolidatedWeather> GetLocationWeatherForDateAsync(DateTime date, string location)
        {
            if (CheckForInternetConnection())
            {
                string woeid = await GetWoeid(location) ?? DemoPurposeWoeid;


                var response = await _httpClient.GetAsync($"location/{woeid}/{date.Year}/{date.Month}/{date.Day}");


                var responseString = await response.Content.ReadAsStringAsync();
                var listOfModels = JsonConvert.DeserializeObject<ConsolidatedWeather[]>(responseString);
                if (listOfModels == null || listOfModels.Length == 0) return new ConsolidatedWeather();
                var weatherClosestToDate = listOfModels.OrderBy(x => (x.created - date).Ticks).First();
                return weatherClosestToDate;
            }
            
            return new ConsolidatedWeather();


        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
