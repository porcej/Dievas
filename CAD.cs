using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dievas.Models;
using AFD.Dashboard.Models;

namespace Dievas {
	
	public class CAD {

		private readonly ConcurrentDictionary<int, IncidentDto> _incidents = new ConcurrentDictionary<int, IncidentDto>();

		private readonly ConcurrentDictionary<string, UnitDto> _units = new ConcurrentDictionary<string, UnitDto>();

		public IEnumerable<UnitDto> GetUnits() {
			return _units.Values.OrderBy( t => t.RadioName );
		}

		public UnitDto GetUnitByName(string radioName) {
			if (_units.ContainsKey(radioName)) return _units[radioName];
        	return new UnitDto() { RadioName = radioName};
		}

		public void AddOrUpdateUnit(UnitDto unit)
		{
			_units[unit.RadioName] = unit;

		}

		public UnitDto UpdateUnitField(string radioName, string field, string value) {
			UnitDto unit = new UnitDto{ RadioName = radioName};
			if (_units.ContainsKey(radioName)) unit = _units[radioName];
			
			unit[field] = value;
			_units[radioName] = unit;		    
		    
		    return _units[radioName];
		}

		public UnitDto UpdateUnitStatus(string radioName, int statusId) {
			UnitDto unit = new UnitDto { RadioName = radioName, StatusId = statusId};
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
        	return new IncidentDto() { Id = id};
        }

		// We don't do incident # checking here, for the case where we want to add
		// an existing incident with new and or updated data
		public IncidentDto AddIncident(int id, IncidentDto incident){

            if (incident.UnitsAssigned.Count > 1)
                incident.UnitsAssigned = RemoveDuplicateAssignments(incident.UnitsAssigned);

            _incidents[id] = incident;
			return incident;
		}

		// We don't do incident # checking here, for the case where we want to add
		// an existing incident with new and or updated data
		public IncidentDto AddIncident(IncidentDto incident){
			int id = incident.Id;

			if (incident.UnitsAssigned.Count>1)
                incident.UnitsAssigned = RemoveDuplicateAssignments(incident.UnitsAssigned);

            _incidents[id] = incident;
			return incident;
		}

		public List<UnitAssignmentDto> RemoveDuplicateAssignments (List<UnitAssignmentDto> unitAssignments)
		{
            return unitAssignments
				.GroupBy(unit => unit.RadioName)
				.Select(group => group.OrderByDescending(assignment => assignment.StartDateTime).First())
				.ToList();
        }

		// Update incident Field - any updates, we copy out the value, make the update
		// return the value to the dict
		public IncidentDto UpdateIncidentField(int id, string field, string value){

			IncidentDto incident = new IncidentDto {Id = id };

			if (_incidents.ContainsKey(id)) incident = _incidents[id];
			incident[field] = value;
			_incidents[id] = incident;		    
		    return _incidents[id];
		}

		public IncidentDto AddOrUpdateIncidentUnit(int id, UnitAssignmentDto unit) {

			// Update global unit
			UnitDto globalUnit = GetUnitByName(unit.RadioName);
			globalUnit.StatusId = unit.StatusId;
			globalUnit.StatusCode = unit.StatusCode;
            globalUnit.IncidentId = unit.IncidentId;
            AddOrUpdateUnit(globalUnit);

			// Update unit on incident
			IncidentDto incident = new IncidentDto {Id = id};

			if (_incidents.ContainsKey(id)) incident = _incidents[id];

			// 2023-01-27 - Removed the use of IndexOf until such time as DTO object support IEquatable<T>
            // var unitKey = incident.UnitsAssigned.IndexOf(unit);
            var unitKey = (incident.UnitsAssigned == null ? -1 : incident.UnitsAssigned.FindIndex(u => u.RadioName == unit.RadioName));
            
            if (unitKey < 0) {
                incident.UnitsAssigned.Add(unit);
            } else {
                incident.UnitsAssigned[unitKey] = unit;
            }
            _incidents[id] = incident;
            return incident;
		}

		public IncidentDto AddOrUpdateIncidentUnit(int id, UnitDto unit) {

			// Update global unit
			AddOrUpdateUnit(unit);

            UnitAssignmentDto assignedUnit = new UnitAssignmentDto
            {
                RadioName = unit.RadioName,
                StatusId = unit.StatusId,
				StatusCode = unit.StatusCode,
                IncidentId = id
            };


            // Update unit on incident
            IncidentDto incident = new IncidentDto {Id = id};

			if (_incidents.ContainsKey(id)) incident = _incidents[id];

			// 2023-01-27 - Removed the use of IndexOf until such time as DTO object support IEquatable<T>
            // var unitKey = incident.UnitsAssigned.IndexOf(assignedUnit);
            // var unitKey = incident.UnitsAssigned.FindIndex(u => u.RadioName == assignedUnit.RadioName);
            var unitKey = (incident.UnitsAssigned == null ? -1 : incident.UnitsAssigned.FindIndex(u => u.RadioName == assignedUnit.RadioName));
            
            if (unitKey < 0) {
                incident.UnitsAssigned.Add(assignedUnit);
            } else {
                incident.UnitsAssigned[unitKey] = assignedUnit;
            }
            _incidents[id] = incident;
            return incident;
		}

		public IncidentDto AddOrUpdateIncidentComment(int id, CommentDto comment){
			IncidentDto incident = new IncidentDto {Id = id};
			
			if (_incidents.ContainsKey(id)) incident = _incidents[id];

			// 2023-01-27 - Removed the use of IndexOf until such time as DTO Objects to IEquatable<T>
			// var commentKey = incident.Comments.IndexOf(comment);
			// var commentKey = incident.Comments.FindIndex(x => x.Id == comment.Id);
			var commentKey = (incident.Comments == null ? -1 : incident.Comments.FindIndex(x => x.Id == comment.Id));



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

		public int PopulateUnitList(IEnumerable<UnitDto> initialUnits)
		{
			foreach (var unit in initialUnits)
			{
				_units[unit.RadioName] = unit;
			}
			return initialUnits.Count();
		}

		/// Remove's all closed incidents older than now - ageToKeep old
		/// If ageToKeep is null, removes all closed incidents
		public int RemoveClosedIncidents(TimeSpan? ageToKeep = null) {
			if (!ageToKeep.HasValue) ageToKeep = TimeSpan.Zero;
			DateTime ? timeThreshold = DateTime.Now - ageToKeep;

			var oldIncidents = _incidents.Where(i =>
				timeThreshold >= i.Value.IncidentEndDateTime
				// timeThreshold.GreaterThanOrEqual<DateTime>(i.Value.IncidentEndDateTime)
			).ToArray();

			/// TODO: ADD Logging for count
			/// _logger.log(LogLevel.Debug, "{} incidents older than {} slated for removal.", oldIncidents.length, timeThreshold);

			int count = 0;
			foreach (var oldIncident in oldIncidents) {
				if (this._removeIncident(oldIncident)) count++;
			}

			return count;
		}

		/// Remove's all closed incidents older than now - ABS(hours), hours old
		/// If hours is zero, removes all closed incidents
		public int RemoveClosedIncidents(double hours = 0) {
			if (hours > 0) hours = hours * -1;

			DateTime timeThreshold = DateTime.Now.AddHours(hours);

			var oldIncidents = _incidents.Where(i =>
				timeThreshold >= i.Value.IncidentEndDateTime
				// timeThreshold.GreaterThanOrEqual<DateTime>(i.Value.IncidentEndDateTime)
			).ToArray();

			/// TODO: ADD Logging for count
			/// _logger.log(LogLevel.Debug, "{} incidents older than {} slated for removal.", oldIncidents.length, timeThreshold);

			int count = 0;
			foreach (var oldIncident in oldIncidents) {
				if (this._removeIncident(oldIncident)) count++;
			}

			return count;
		}
		private bool _removeIncident(KeyValuePair<int, IncidentDto> incident){
				if (_incidents.TryRemove(incident)) {
					/// TODO: ADD Logging for incident removal
					/// _logger.log(LogLevel.Debug, "Removed incident # {}", oldIncident.key);
					return true;
				} else {
					/// TODO: ADD Logging for incident removal failure
					/// _logger.log(LogLevel.Warn, "Failed to remove incident # {}", oldIncident.key);
				}
				return false;
		}

		private bool _removeIncident(int id, IncidentDto incident) {
				if (_incidents.TryRemove(id, out incident)) {
					/// TODO: ADD Logging for incident removal
					/// _logger.log(LogLevel.Debug, "Removed incident # {}", oldIncident.key);
					return true;
				} else {
					/// TODO: ADD Logging for incident removal failure
					/// _logger.log(LogLevel.Warn, "Failed to remove incident # {}", oldIncident.key);
				}
				return false;
		}

		public bool RemoveIncident(int id) {
			if (_incidents.ContainsKey(id)) {
				IncidentDto incident = _incidents[id];
				if (this._removeIncident(id, incident)) return true;
			}
			return false;
		}
	}
}