using System;
using System.Collections.Generic;
using Dievas.Models.Telestaff;
using Newtonsoft.Json;

namespace Dievas.Models.Staffing {

   	/// <summary>
    ///     Class <c>StaffingRecord</c> Staffing record for dashboarding purposes
    /// </summary>
	public class StaffingRecord {

	    public StaffingRecord(RosterRecord rr) {
			RosterDate = rr.StaffingCalendarDa;
	    	InstitutionNotes = rr.InstitutionDispatchAttachCh ?? "";
	    	AgencyNotes = rr.AgencyDispatchAttachCh ?? "";
	    	RegionNotes = rr.RegionDispatchAttachCh ?? "";
	    	ShiftNotes = rr.ShiftDispatchAttachCh ?? "";
	    	StationNotes = rr.StationDispatchAttachCh ?? "";
	    	UnitAbbreviation = rr.PhysicalUnitAbrvCh ?? "";
	    	UnitName = rr.PhysicalUnitNameCh ?? "";
	    	UnitNotes = rr.PunitDispatchAttachCh ?? "";
	    	
	    	

	    	Badge = rr.RscEmployeeIDCh ?? "";
	    	Duration = rr.StaffingDurationIn;
	    	EndTime = rr.StaffingEndDt;
	    	ExceptionCode = rr.WstatPayrollCh ?? "";
	    	IsRequest = rr.Request;
	    	IsVacant = (rr.RecordType.ToUpper() == "VACANCY");	// this is a staffing record so it can not be vacant
	    	Name = rr.RscMasterNameCh ?? "";
	    	Specialties = rr.RscXSpecNames ?? "";
	    	StartTime = rr.StaffingStartDt;
	    	DetailCode = rr.StaffingDetailCh ?? "";
	    }

	    /// <summary>
	    /// 	Roster Date
	    ///	</summary>
	    [JsonProperty("dateTitle")]
	    public DateTime RosterDate { get; set; }

	    /// <summary>
	    /// 	Institution Name
	    ///	</summary>
	    [JsonProperty("institutionName")]
	    public string InstitutionName { get; set; }

	    /// <summary>
	    /// 	Institution Notes
	    ///	</summary>
	    [JsonProperty("institutionNotes")]
	    public string InstitutionNotes { get; set; }

	    /// <summary>
	    /// 	Agency Name
	    ///	</summary>
	    [JsonProperty("agencyName")]
	    public string AgencyName { get; set; }

	    /// <summary>
	    /// 	Agency Notes
	    ///	</summary>
	    [JsonProperty("agencyNotes")]
	    public string AgencyNotes { get; set; }

	    /// <summary>
	    /// 	Region Name - formally known as batallion in Festis
	    ///	</summary>
	    [JsonProperty("regionName")]
	    public string RegionName { get; set; }

	    /// <summary>
	    /// 	Region Notes - formally known as batallion in Festis
	    ///	</summary>
	    [JsonProperty("regionNotes")]
	    public string RegionNotes { get; set; }

	    /// <summary>
	    /// 	Shift Name
	    ///	</summary>
	    [JsonProperty("shiftName")]
	    public string ShiftName { get; set; }

	    /// <summary>
	    /// 	Shift Notes
	    ///	</summary>
	    [JsonProperty("shiftNotes")]
	    public string ShiftNotes { get; set; }

	    /// <summary>
	    /// 	Station Name
	    ///	</summary>
	    [JsonProperty("stationName")]
	    public string StationName { get; set; }

	    /// <summary>
	    /// 	Station Notes
	    ///	</summary>
	    [JsonProperty("stationNotes")]
	    public string StationNotes { get; set; }

	    /// <summary>
	    /// 	Unit Name
	    ///	</summary>
	    [JsonProperty("unitName")]
	    public string UnitName { get; set; }

	    /// <summary>
	    /// 	Unit Notes
	    ///	</summary>
	    [JsonProperty("unitNotes")]
	    public string UnitNotes { get; set; }

	    /// <summary>
	    /// 	Unit Abbreviation
	    ///	</summary>
	    [JsonProperty("unitAbbreviation")]
	    public string UnitAbbreviation { get; set; }

	    /// <summary>
	    ///		badge - Badge number for person assocaited with this record
	    ///	</summary>
	    [JsonProperty("badge")]
	    public string Badge { get; set; }

	    /// <summary>
	    ///		duration - Staffing duration for this record in hours
	    ///	</summary>
	    [JsonProperty("duration")]
	    public double Duration { get; set; }

	   	/// <summary>
	    ///		EndTime - end of this staffing duration
	    ///	</summary>
	    [JsonProperty("endTime")]
	    public DateTime EndTime { get; set; }

	   	/// <summary>
	    ///		ExceptionCode - Exception code attached to this staffing record
	    ///	</summary>
	    [JsonProperty("ExceptionCode")]
	    public string ExceptionCode { get; set; }

	   	/// <summary>
	    ///		IsRequest - true if this record is a non-approved request
	    ///	</summary>
	    [JsonProperty("isRequest")]
	    public bool IsRequest { get; set; }

	   	/// <summary>
	    ///		IsVacant - true if this record does not have a person attached to it
	    ///	</summary>
	    [JsonProperty("isVacant")]
	    public bool IsVacant { get; set; }

	   	/// <summary>
	    ///		IsWorking - true if this record is for a working type code
	    ///	</summary>
	    [JsonProperty("isWorking")]
	    public bool IsWorking { get; set; } 

		/// <summary>
	    ///		Name - Name for the person associated with this record
	    ///	</summary>
	    [JsonProperty("name")]
	    public string Name { get; set; }

		/// <summary>
	    ///		Notes - associated with this specific position
	    ///	</summary>
	    [JsonProperty("notes")]
	    public string Notes { get; set; }

		/// <summary>
	    ///		Specialties - specialites for the person associated with this record
	    ///	</summary>
	    [JsonProperty("specialities")]
	    public string Specialties { get; set; }

	   	/// <summary>
	    ///		StartTime - start of this staffing duration
	    ///	</summary>
	    [JsonProperty("startTime")]
	    public DateTime StartTime { get; set; }

	   	/// <summary>
	    ///   Position uniqie identifier
	    /// </summary>
	    [JsonProperty("id")]
	    public int Id { get; set; }

	    /// <summary>
	    ///   title - Position name
	    /// </summary>
	    [JsonProperty("title")]
	    public string Title { get; set; }

	    /// <summary>
	    ///   Position name in an organizational hierachy string
	    /// </summary>
	    [JsonProperty("positionDisplayName")]
	    public string PositionDisplayName { get; set; }

	    /// <summary>
	    ///   Position name in an organizational hierachy string
	    /// </summary>
	    [JsonProperty("workCode")]
	    public string WorkCode { get; set; }

	    /// <summary>
	    ///   Detail Code
	    /// </summary>
	    [JsonProperty("detailCode")]
	    public string DetailCode { get; set; }
    }
}
