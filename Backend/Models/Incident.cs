using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models {
    
    public class Incident {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string Jurisdiction { get; set; }
        public string IncidentType { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string County { get; set; }
        public string LocationType { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string CrossStreet { get; set; }
        public string CommandChannel { get; set; }
        public string PrimaryTacChannel { get; set; }
        public string AlternateTacChannel { get; set; }
        public string CallDisposition { get; set; }
        public DateTime IncidentStartTime { get; set; }
        public DateTime IncidentEndTime { get; set; }
        public List<Comment> Comments { get; set; }
        public List<AssignedUnit> Units { get; set; }

        public override bool Equals(Object obj) {
            return (obj is Incident) && ((Incident)obj).Id == Id;
        }
     
        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}
