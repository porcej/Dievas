using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models {

    public class Unit {
        // public int id { get; set; }
        public string RadioName { get; set; }
        public int StatusId { get; set; }
        public string DestinationLocation { get; set; }
        public string HomeStation { get; set; }
        // AVL Info below
        public short Speed { get; set; }
        public string CurrentLocation { get; set; }
        public int Longitude { get; set; }  // Latitude * 0.000001f;
        public int Latitude { get; set; }   // Longitude * -0.000001f;
        public string Heading { get; set; }
        public int Altitude { get; set; }
        public DateTime LastAVLUpdate { get; set; }

        public override bool Equals(Object obj) {
            return (obj is Unit) && ((Unit)obj).RadioName == RadioName;
        }
     
        public override int GetHashCode() {
            return RadioName.GetHashCode();
        }
    }
}
