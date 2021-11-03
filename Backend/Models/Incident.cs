using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models {
    
    public class Incident {
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
    }
}
