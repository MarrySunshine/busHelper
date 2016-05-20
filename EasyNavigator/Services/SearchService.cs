using EasyNavigator.Libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Popups;

namespace EasyNavigator.Services
{
    public class SearchService
    {
        static private SearchService instance = null;
        static public SearchService Instance
        {
            get
            {
                return instance == null ? (instance = new SearchService()) : instance;
            }
        }

        private Request request = new Request();
        //对于keyword，搜索地点，把相近的搜索结果放到list中返回。
        public async Task<List<Models.AddressModel>> getResultByKeyWord(string keyWord)
        {
            var param_list = new List<KeyValuePair<string, string>>();
            param_list.Add(new KeyValuePair<string, string>("key", Config.Key));
            param_list.Add(new KeyValuePair<string, string>("keywords", keyWord));
            param_list.Add(new KeyValuePair<string, string>("city", Config.locationCity));
            param_list.Add(new KeyValuePair<string, string>("offset", "30"));

            var addressList = new List<Models.AddressModel>();
            await request.getAsync(Config.SearchAPI, param_list,
                (header, body) =>
                {
                    var obj = JsonObject.Parse(body);
                    if (obj["info"].GetString() == "OK")
                    {
                        foreach (var item in obj["pois"].GetArray())
                        {
                            var _ = item.GetObject();
                            addressList.Add(new Models.AddressModel(_["id"].GetString(), _["name"].GetString(),
                                _["type"].GetString(), _["address"].GetString(), _["location"].GetString().Split(',')[0],
                                _["location"].GetString().Split(',')[1]));
                        }
                    }
                },
                async (header, err) =>
                {
                    await new MessageDialog(err.ToString()).ShowAsync();
                });

            return addressList;
        }
    }
}
