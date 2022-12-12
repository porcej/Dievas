using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using AFD.Dashboard.Models;

namespace Dievas {
	
	public class CAD {

		private readonly ConcurrentDictionary<int, IncidentDto> _incidents = new ConcurrentDictionary<int, IncidentDto>();

		private readonly ConcurrentDictionary<string, UnitDto> _units = new ConcurrentDictionary<string, UnitDto>();

		public IEnumerable GetUnits() {
			return _units.Values.OrderBy( t => t.RadioName );
		}

		public IEnumerable GetUnitsByStation(string station) {
			return _units.Values.Where(t => t.HomeStation == station).OrderBy( t => t.RadioName );
		}

		public UnitDto GetUnitByName(string radioName) {
			if (_units.ContainsKey(radioName)) return _units[radioName];
        	return new UnitDto();
		}

		public void AddOrUpdateUnit(UnitDto unit)
		{
			_units[unit.RadioName] = unit;

		}

		public UnitDto UpdateUnitField(string radioName, string field, string value) {
			UnitDto unit = new UnitDto{};
			if (_units.ContainsKey(radioName)) unit = _units[radioName];
			
			unit[field] = value;
			_units[radioName] = unit;		    
		    
		    return _units[radioName];
		}

		public UnitDto UpdateUnitStatus(string radioName, int statusId) {
			UnitDto unit = new UnitDto {};
			if (_units.ContainsKey(radioName)) unit = _units[radioName];
			
			unit.StatusId = statusId;
			_units[radioName] = unit;		    
		    
		    return _units[radioName];
		}

		public IEnumerable GetIncidents() {
            return _incidents.Values.OrderBy( t => t.Id );
        }

        public IEnumerable GetActiveIncidents() {
            return _incidents.Values.Where(t => t.IsActive).OrderBy( t => t.Id );
        }

        public IncidentDto GetIncident(int id) {
        	if (_incidents.ContainsKey(id)) return _incidents[id];
        	return new IncidentDto();
        }

		// We don't do incident # checking here, for the case where we want to add
		// an existing incident with new and or updated data
		public IncidentDto AddIncident(int id, IncidentDto incident){
			_incidents[id] = incident;
			return incident;
		}

		// We don't do incident # checking here, for the case where we want to add
		// an existing incident with new and or updated data
		public IncidentDto AddIncident(IncidentDto incident){
			int id = incident.Id;
			_incidents[id] = incident;
			return incident;
		}

		// Update incident Field - any updates, we copy out the value, make the update
		// return the value to the dict
		public IncidentDto UpdateIncidentField(int id, string field, string value){

            IncidentDto incident;

			if (_incidents.TryGetValue(id, out incident))
			{
				incident[field] = value;
				_incidents[id] = incident;
				return _incidents[id];
			}
			return null;
		}

		public IncidentDto AddOrUpdateIncidentUnit(int id, UnitAssignmentDto unit) {

			// Update global unit
			UnitDto globalUnit = GetUnitByName(unit.RadioName);
			globalUnit.StatusId = unit.StatusId;
			AddOrUpdateUnit(globalUnit);

            // Update unit on incident
            IncidentDto incident;

			if (_incidents.TryGetValue(id, out incident))
			{
                incident.UnitsAssigned ??= new List<UnitAssignmentDto>();
                var unitKey = incident.UnitsAssigned.IndexOf(unit);

				if (unitKey < 0)
				{
					incident.UnitsAssigned.Add(unit);
				}
				else
				{
					incident.UnitsAssigned[unitKey] = unit;
				}
				_incidents[id] = incident;
				return incident;
			}
			return null;
		}

		public IncidentDto AddOrUpdateIncidentUnit(int id, UnitDto unit) {

			// Update global unit
			AddOrUpdateUnit(unit);

			UnitAssignmentDto assignedUnit = new UnitAssignmentDto { 
				RadioName = unit.RadioName,
				StatusId = unit.StatusId,
				IncidentId = id
			};

			// Update unit on incident
			IncidentDto incident;

			if (_incidents.TryGetValue(id, out incident))
			{
				incident.UnitsAssigned ??= new List<UnitAssignmentDto>();

				var unitKey = incident.UnitsAssigned.IndexOf(assignedUnit);

				if (unitKey < 0)
				{
					incident.UnitsAssigned.Add(assignedUnit);
				}
				else
				{
					incident.UnitsAssigned[unitKey] = assignedUnit;
				}
				_incidents[id] = incident;
				return incident;
			}
			return null;
		}

		public IncidentDto AddOrUpdateIncidentComment(int id, CommentDto comment){
			IncidentDto incident;

			if (_incidents.TryGetValue(id, out incident))
			{
				incident.Comments ??= new List<CommentDto>();
				
				var commentKey = incident.Comments.IndexOf(comment);

				if (commentKey < 0) {
					incident.Comments.Add(comment);
				} else {
					incident.Comments[commentKey] = comment;
				}
				_incidents[id] = incident;
				return incident;
			}
			return null;
		}

		public int IncidentCount() {
			return _incidents.Count;
		}

		public int PopulateUnitList(IEnumerable<UnitDto> initialUnits)
		{
			foreach (var unit in initialUnits)
			{
				_units[unit.RadioName] = unit;
			}
			return initialUnits.Count();
		}
		
	}
}