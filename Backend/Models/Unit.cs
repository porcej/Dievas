using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models {

    public class Unit {
        // public int id { get; set; }
        public string radioName { get; set; }
        public int statusId { get; set; }
        public string destinationLocation { get; set; }
        public string HomeStation { get; set; }
        // AVL Info below
        public short Speed { get; set; }
        public string currentLocation { get; set; }
        public int longitude { get; set; }  // Latitude * 0.000001f;
        public int latitude { get; set; }   // Longitude * -0.000001f;
        public string Heading { get; set; }
        public int Altitude { get; set; }
        public DateTime LastAVLUpdate { get; set; }
    }
}
