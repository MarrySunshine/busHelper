using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusHelper.Models
{
    public enum BusArriveState : int
    {
        Not_Arrived,
        Arrived
    }
    public class BusModel
    {
        private string city;
        private int id;
        private int station;
        private BusArriveState state;
        private int distance;
        private int reporTime;

        public string City { get { return city; } }
        public int Id { get { return id; } }
        public int Station { get { return station; } }
        public BusArriveState State { get { return state; } }
        public int Distance { get { return distance; } }
        public int ReporTime { get { return reporTime; } }

        public BusModel(string city, int id, int station, BusArriveState state, int distance, int reporTime)
        {
            this.city = city;
            this.id = id;
            this.station = station;
            this.state = state;
            this.distance = distance;
            this.reporTime = reporTime;
        }

    }
}
