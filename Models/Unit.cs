using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Backend.Models {

    public class Unit {

        // Public indexer
        public object this[string propertyName] {
            get {
                Type myType = typeof(Unit);                   
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set {
                Type myType = typeof(Unit);                   
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);

                // If the provided value is a string, attempt to convert it
                if (value is string){
                    if (myPropInfo.PropertyType == typeof(double)) 
                        myPropInfo.SetValue(this, Convert.ToDouble(value), null);
                    if (myPropInfo.PropertyType == typeof(int)) 
                        myPropInfo.SetValue(this, Convert.ToInt32(value), null);
                    if (myPropInfo.PropertyType == typeof(DateTime)) 
                        myPropInfo.SetValue(this, DateTime.Parse(Convert.ToString(value)), null);
                } else {
                    myPropInfo.SetValue(this, value, null);
                }
            }
         }

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

        public override bool Equals(Object obj) {
            return (obj is Unit) && ((Unit)obj).radioName == radioName;
        }
     
        public override int GetHashCode() {
            return radioName.GetHashCode();
        }
    }
}
