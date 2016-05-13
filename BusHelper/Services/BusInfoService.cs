using BusHelper.Models;
using BusHepler.Libs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Popups;

namespace BusHelper.Services
{
    public class BusInfoService
    {
        static private BusInfoService instance = null;
        static public BusInfoService Instance
        {
            get
            {
                return instance == null ? (instance = new BusInfoService()) : instance;
            }
        }

        private ConfigService configService = ConfigService.Instance;
        
        private CityModel parseJsonObjectToCityModel(string cityName, JsonObject json)
        {
            var buses = new List<BusModel>();
            var stations = new List<StationModel>();
            var data = json["data"].GetObject();

            foreach (var item in data["buses"].GetArray())
            {
                var bus = item.GetObject();
                buses.Add(new BusModel(cityName,
                    (int)bus["busId"].GetNumber(),
                    (int)bus["station"].GetNumber(),
                    (BusArriveState)(int)bus["state"].GetNumber(),
                    (int)bus["distance"].GetNumber(),
                    (int)bus["reporTime"].GetNumber())
                );
            }
            
            foreach (var item in data["stations"].GetArray())
            {
                var station = item.GetObject();
                stations.Add(new StationModel((int)station["station"].GetNumber(),station["stateName"].GetString()));
            }

            return new CityModel(cityName, ref stations, ref buses);
        }

        public async Task<CityModel> getBusInfo(string city, string busLineName = null, string direction = "0")
        {
            var request = new Request();
            request.Headers.Add("apikey", configService.apikey);

            var parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("city", city),
                new KeyValuePair<string, string>("direction", direction)
            };
            if (busLineName != null)
            {
                parameters.Add(new KeyValuePair<string, string>("bus", busLineName));
            }

            CityModel result = null;

            await request.getAsync(configService.interfaceUrl, parameters,
                (header, body) =>
                {
                    result = parseJsonObjectToCityModel(city, JsonObject.Parse(body));
                    return;
                },
                async (header, err) =>
                {
                    await new MessageDialog(err.ToString()).ShowAsync();
                });

            return result;
        }
    }
}
