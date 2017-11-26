using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FriendOrganizer.UI.View.Services.WeatherModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FriendOrganizer.UI.View.Services
{
    public class WeatherService : IWeatherService
    {
        private static HttpClient httpClient;
        private const string DemoPurposeWoeid = "890869";

        public WeatherService()
        {
            httpClient= new HttpClient();
            httpClient.BaseAddress = new Uri("https://www.metaweather.com/api/");


        }

        internal async Task<string> GetWoeid(string location)
        {           

            var response = await httpClient.GetAsync($"location/search/?query={location}");
            var responseString = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ApiLocation[]>(responseString);

            return model[0].woeid.ToString();

            
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
            string woeid = await GetWoeid(location) ?? DemoPurposeWoeid;


                var response = await httpClient.GetAsync($"location/{woeid}/{date.Year}/{date.Month}/{date.Day}");


                var responseString = await response.Content.ReadAsStringAsync();
                var listOfModels = JsonConvert.DeserializeObject<ConsolidatedWeather[]>(responseString);
            if (listOfModels != null && listOfModels.Length !=0)
            {
                var weatherClosestToDate = listOfModels.OrderBy(x => (x.created - date).Ticks).First();

                return weatherClosestToDate;
            }
            return new ConsolidatedWeather();


        }

        public async Task<ConsolidatedWeather> GetLocationWeatherForDateAsync(DateTime date)
        {
            string woeid = DemoPurposeWoeid;


            try
            {
                var response = await httpClient.GetAsync($"location/{woeid}/{date.Year}/{date.Month}/{date.Day}");

                var responseString = await response.Content.ReadAsStringAsync();
                var listOfModels = JsonConvert.DeserializeObject<ConsolidatedWeather[]>(responseString);
                var weatherClosestToDate = listOfModels.OrderBy(x => (x.created - date).Ticks).First();

                return weatherClosestToDate;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
