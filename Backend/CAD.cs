using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend {
	
	public class CAD {

		private readonly ConcurrentDictionary<int, Incident> _incidents = new ConcurrentDictionary<int, Incident>();


		public event EventHandler<CadEventArgs> IncidentAdded;
		public event EventHandler<CadEventArgs> IncidentChanged;

		public IEnumerable GetIncidents() {
            return _incidents.Values.OrderBy( t => t.Id );
        }

        public IEnumerable GetActiveIncidents() {
            return _incidents.Values.Where(t => t.Active).OrderBy( t => t.Id );
        }

        public Incident GetIncident(int id) {
        	if (_incidents.ContainsKey(id)) return _incidents[id];
        	return new Incident();
        }

		// We don't do incident # checking here, for the case where we want to add
		// an existing incident with new and or updated data
		public Incident AddIncident(int id, Incident incident){
			_incidents[id] = incident;
			return incident;
		}

		// We don't do incident # checking here, for the case where we want to add
		// an existing incident with new and or updated data
		public Incident AddIncident(Incident incident){
			int id = incident.Id;
			_incidents[id] = incident;
			return incident;
		}

		public Incident AddOrUpdateIncident(Incident incident){
			if (_incidents.ContainsKey(incident.Id)){
				_incidents[incident.Id] = incident;
				OnIncidentEvent(new CadEventArgs { EventType = "IncidentChanged", IncidentId = incident.Id});
			}else{
				int id = incident.Id;
				_incidents[id] = incident;
				OnIncidentEvent(new CadEventArgs { EventType = "IncidentAdded", IncidentId = incident.Id});
			}
			return incident;
		}

    private void OnIncidentEvent(CadEventArgs cadEventArgs)
    {
      switch (cadEventArgs.EventType)
			{
					case"IncidentAdded":
						IncidentAdded?.Invoke(this, cadEventArgs);
						break;
					case "IncidentChanged":
						IncidentChanged?.Invoke(this, cadEventArgs);
						break;
					default:
						break;
			}
    }

    // Update incident Field - any updates, we copy out the value, make the update
    // return the value to the dict
    public Incident UpdateIncidentField(int id, string field, string value){

			Incident incident = new Incident {};

			if (_incidents.ContainsKey(id)) incident = _incidents[id];

		    switch (field) {
		        case "active":
		            incident.Active = (value.ToLower() == "true");
		            break;
		        case "jurisdiction":
		            incident.Jurisdiction = value;
		            break;
		        case "incidentType":
		            incident.IncidentType = value;
		            break; 
		        case "LocationName":
		            incident.LocationName = value;
		            break; 
		        case "address":
		            incident.Address = value;
		            break; 
		        case "apartment":
		            incident.Apartment = value;
		            break; 
		        case "city":
		            incident.City = value;
		            break; 
		        case "state":
		            incident.State = value;
		            break; 
		        case "postalCode":
		            incident.PostalCode = value;
		            break; 
		        case "county":
		            incident.County = value;
		            break; 
		        case "locationType":
		            incident.LocationType = value;
		            break; 
		        case "crossStreet":
		            incident.CrossStreet = value;
		            break; 
		        case "commandChannel":
		            incident.CommandChannel = value;
		            break; 
		        case "primaryTACChannel":
		            incident.PrimaryTacChannel = value;
		            break; 
		        case "alternateTACChannel":
		            incident.AlternateTacChannel = value;
		            break; 
		        case "callDisposition":
		            incident.CallDisposition = value;
		            break;
		        case "longitude":
		            incident.Longitude = Convert.ToDouble(value);
		            break;
		        case "latitude":
		            incident.Latitude = Convert.ToDouble(value);
		            break;
		        case "incidentStartTime":
		            incident.IncidentStartTime = DateTime.Parse(value);
		            break;
		        case "incidentEndTime":
		            incident.IncidentEndTime = DateTime.Parse(value);
		            break;
		    }
			_incidents[id] = incident;		    
		    return _incidents[id];
		}

		public Incident AddOrUpdateIncidentUnit(int id, AssignedUnit unit) {
			Incident incident = new Incident {};

			if (_incidents.ContainsKey(id)) incident = _incidents[id];

            var unitKey = incident.Units.IndexOf(unit);
            
            if (unitKey < 0) {
                incident.Units.Add(unit);
            } else {
                incident.Units[unitKey] = unit;
            }
            _incidents[id] = incident;
            return incident;
		}

		public Incident AddOrUpdateIncidentComment(int id, Comment comment){
			Incident incident = new Incident {};

			if (_incidents.ContainsKey(id)) incident = _incidents[id];

			var commentKey = incident.Comments.IndexOf(comment);

            if (commentKey < 0) {
                incident.Comments.Add(comment);
            } else {
                incident.Comments[commentKey] = comment;
            }
            _incidents[id] = incident;
            return incident;
		}

		public int IncidentCount() {
			return _incidents.Count;
		}
	}

  public class CadEventArgs : EventArgs
  {
		public string EventType {get;set;}
		public int IncidentId {get;set;}
  }
}