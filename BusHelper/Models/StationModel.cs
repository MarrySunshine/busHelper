using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusHelper.Models
{
    public class StationModel
    {
        private int id;
        private string name;

        public int Id { get { return id; } }
        public string Name { get { return name; } }

        public StationModel(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
