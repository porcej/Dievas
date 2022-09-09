using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Dievas {
	
	public class CAD {

		private readonly ConcurrentDictionary<int, Incident> _incidents = new ConcurrentDictionary<int, Incident>();

		private readonly ConcurrentDictionary<string, Unit> _units = new ConcurrentDictionary<string, Unit>();

		public IEnumerable GetUnits() {
			return _units.Values.OrderBy( t => t.radioName );
		}

		public IEnumerable GetUnitsByStation(string station) {
			return _units.Values.Where(t => t.HomeStation == station).OrderBy( t => t.radioName );
		}

		public Unit GetUnitByName(string radioName){
			if (_units.ContainsKey(radioName)) return _units[radioName];
        	return new Unit();
		}

		public void AddOrUpdateUnit(Unit unit){
			_units[unit.radioName] = unit;
		}

		public IEnumerable GetIncidents() {
            return _incidents.Values.OrderBy( t => t.id );
        }

        public IEnumerable GetActiveIncidents() {
            return _incidents.Values.Where(t => t.active).OrderBy( t => t.id );
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
			int id = incident.id;
			_incidents[id] = incident;
			return incident;
		}

		// Update incident Field - any updates, we copy out the value, make the update
		// return the value to the dict
		public Incident UpdateIncidentField(int id, string field, string value){

			Incident incident = new Incident {};

			if (_incidents.ContainsKey(id)) incident = _incidents[id];

		    switch (field) {
		        case "active":
		            incident.active = (value.ToLower() == "true");
		            break;
		        case "jurisdiction":
		            incident.jurisdiction = value;
		            break;
		        case "incidentType":
		            incident.incidentType = value;
		            break; 
		        case "LocationName":
		            incident.LocationName = value;
		            break; 
		        case "address":
		            incident.address = value;
		            break; 
		        case "apartment":
		            incident.apartment = value;
		            break; 
		        case "city":
		            incident.city = value;
		            break; 
		        case "state":
		            incident.state = value;
		            break; 
		        case "postalCode":
		            incident.postalCode = value;
		            break; 
		        case "county":
		            incident.county = value;
		            break; 
		        case "locationType":
		            incident.locationType = value;
		            break; 
		        case "crossStreet":
		            incident.crossStreet = value;
		            break; 
		        case "commandChannel":
		            incident.commandChannel = value;
		            break; 
		        case "primaryTACChannel":
		            incident.primaryTACChannel = value;
		            break; 
		        case "alternateTACChannel":
		            incident.alternateTACChannel = value;
		            break; 
		        case "callDisposition":
		            incident.callDisposition = value;
		            break;
		        case "longitude":
		            incident.longitude = Convert.ToDouble(value);
		            break;
		        case "latitude":
		            incident.latitude = Convert.ToDouble(value);
		            break;
		        case "incidentStartTime":
		            incident.incidentStartTime = DateTime.Parse(value);
		            break;
		        case "incidentEndTime":
		            incident.incidentEndTime = DateTime.Parse(value);
		            break;
		    }
			_incidents[id] = incident;		    
		    return _incidents[id];
		}

		public Incident AddOrUpdateIncidentUnit(int id, AssignedUnit unit) {

			// Update global unit
			Unit globalUnit = GetUnitByName(unit.radioName);
			globalUnit.statusId = unit.statusId;
			AddOrUpdateUnit(globalUnit);

			// Update unit on incident
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

		public Incident AddOrUpdateIncidentUnit(int id, Unit unit) {

			// Update global unit
			AddOrUpdateUnit(unit);

			AssignedUnit assignedUnit = new AssignedUnit {};

			assignedUnit.radioName = unit.radioName;
			assignedUnit.statusId = unit.statusId;


			// Update unit on incident
			Incident incident = new Incident {};

			if (_incidents.ContainsKey(id)) incident = _incidents[id];

            var unitKey = incident.Units.IndexOf(assignedUnit);
            
            if (unitKey < 0) {
                incident.Units.Add(assignedUnit);
            } else {
                incident.Units[unitKey] = assignedUnit;
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
}