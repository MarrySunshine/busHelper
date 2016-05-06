using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusHelper.Models
{
    public class CityModel
    {
        private string name;
        private List<StationModel> stations;
        private List<BusModel> buses;

        public string Name { get { return name; } }
        public List<StationModel> Stations { get { return stations; } }
        public List<BusModel> Buses { get { return buses; } }

        public CityModel(string name, ref List<StationModel> stations, ref List<BusModel> buses)
        {
            this.name = name;
            this.stations = stations;
            this.buses = buses;
        }
    }
}
