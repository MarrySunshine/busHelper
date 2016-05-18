using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNavigator.Models
{
    public class AddressModel
    {
        private string id;
        private string name;
        private string type_;
        private string address;
        private string lng;
        private string lat;

        public string Id { get { return id; } }
        public string Name { get { return name; } }
        public string Type { get { return type_; } }
        public string Address { get { return address; } }
        public string Lng { get { return lng; } }
        public string Lat { get { return lat; } }

        public AddressModel(string id, string name, string type, string address, string lng, string lat)
        {
            this.id = id;
            this.name = name;
            this.type_ = type;
            this.address = address;
            this.lng = lng;
            this.lat = lat;
        }

    }
}
