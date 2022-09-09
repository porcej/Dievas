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

		public Unit GetUnitByName(string radioName) {
			if (_units.ContainsKey(radioName)) return _units[radioName];
        	return new Unit();
		}

		public void AddOrUpdateUnit(Unit unit) {
			_units[unit.radioName] = unit;
		}

		public Unit UpdateUnitField(string radioName, string field, string value) {
			Unit unit = new Unit {};
			if (_units.ContainsKey(radioName)) unit = _units[radioName];
			
			unit[field] = value;
			_units[radioName] = unit;		    
		    
		    return _units[radioName];
		}

		public Unit UpdateUnitStatus(string radioName, int statusId) {
			Unit unit = new Unit {};
			if (_units.ContainsKey(radioName)) unit = _units[radioName];
			
			unit.statusId = statusId;
			_units[radioName] = unit;		    
		    
		    return _units[radioName];
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
			incident[field] = value;
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