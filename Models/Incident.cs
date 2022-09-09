using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Backend.Models {
    
    public class Incident {

        // Public indexer
        public object this[string propertyName] {
            get {
                Type myType = typeof(Incident);                   
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                if (myPropInfo is null) return null;
                return myPropInfo.GetValue(this, null);
            }
            set {
                Type myType = typeof(Incident);                   
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);

                if (myPropInfo is null) return;

                // If the provided value is a string, attempt to convert it
                if (value is string){
                    if (myPropInfo.PropertyType == typeof(double)) 
                        myPropInfo.SetValue(this, Convert.ToDouble(value), null);
                    if (myPropInfo.PropertyType == typeof(int)) 
                        myPropInfo.SetValue(this, Convert.ToInt32(value), null);
                    if (myPropInfo.PropertyType == typeof(DateTime)) 
                        myPropInfo.SetValue(this, DateTime.Parse(Convert.ToString(value)), null);
                    if (myPropInfo.PropertyType == typeof(string))
                        myPropInfo.SetValue(this, value, null);  
                } else {
                    myPropInfo.SetValue(this, value, null);
                }
            }
         }

        public int id { get; set; }
        public bool active { get; set; }
        public string jurisdiction { get; set; }
        public string incidentType { get; set; }
        public string LocationName { get; set; }
        public string address { get; set; }
        public string apartment { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string county { get; set; }
        public string locationType { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string crossStreet { get; set; }
        public string commandChannel { get; set; }
        public string primaryTACChannel { get; set; }
        public string alternateTACChannel { get; set; }
        public string callDisposition { get; set; }
        public DateTime incidentStartTime { get; set; }
        public DateTime incidentEndTime { get; set; }
        public List<Comment> Comments { get; set; }
        public List<AssignedUnit> Units { get; set; }

        public override bool Equals(Object obj) {
            return (obj is Incident) && ((Incident)obj).id == id;
        }
     
        public override int GetHashCode() {
            return id.GetHashCode();
        }
    }
}
