using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>RosterRecord</c> Telestaff representation of a roster record
    /// </summary>
	public class RosterRecord {

		/// <summary>
		/// 	Agency abbreviation
		/// </summary>
		[JsonProperty("agencyAbrvCh")]
		public string AgencyAbrvCh { get; set; }

		/// <summary>
		/// 	Agency Dispatch Attach detail
		/// </summary>
		[JsonProperty("agencyDispatchAttachCh")]
		public string AgencyDispatchAttachCh { get; set; }

		/// <summary>
		/// 	Agency externalID
		/// </summary>
		[JsonProperty("agencyExternalIDCh")]
		public string AgencyExternalIDCh { get; set; }

		/// <summary>
		/// 	Check In date time
		/// </summary>
		[JsonProperty("checkInTimestamp")]
		public DateTime CheckInTimestamp { get; set; }

		/// <summary>
		/// 	Display order of the staffing on the roster
		/// </summary>
		[JsonProperty("displayOrderGm")]
		public int DisplayOrderGm { get; set; }

		/// <summary>
		/// 	Institution abbreviation
		/// </summary>
		[JsonProperty("institutionAbrvCh")]
		public string InstitutionAbrvCh { get; set; }

		/// <summary>
		/// 	Institution Dispatch Attach detail
		/// </summary>
		[JsonProperty("institutionDispatchAttachCh")]
		public string InstitutionDispatchAttachCh { get; set; }

		/// <summary>
		/// 	Institution externalID
		/// </summary>
		[JsonProperty("institutionExternalIDCh")]
		public string InstitutionExternalIDCh { get; set; }

		/// <summary>
		/// 	Institution id
		/// </summary>
		[JsonProperty("institutionId")]
		public int InstitutionId { get; set; }

		/// <summary>
		/// 	payDurationIn
		/// </summary>
		[JsonProperty("payDurationIn")]
		public double PayDurationIn { get; set; }

		/// <summary>
		/// 	Physical unit abbreviation
		/// </summary>
		[JsonProperty("physicalUnitAbrvCh")]
		public string PhysicalUnitAbrvCh { get; set; }

		/// <summary>
		/// 	Physical unit name
		/// </summary>
		[JsonProperty("physicalUnitNameCh")]
		public string PhysicalUnitNameCh { get; set; }

		/// <summary>
		/// 	Position Dispatch Attach detail
		/// </summary>
		[JsonProperty("posDispatchAttachCh")]
		public string PosDispatchAttachCh { get; set; }

		/// <summary>
		/// 	Position externalID
		/// </summary>
		[JsonProperty("posExternalIDCh")]
		public string PosExternalIDCh { get; set; }

		/// <summary>
		/// 	Formula id assigned to position
		/// </summary>
		[JsonProperty("posFormulaIDCh")]
		public string PosFormulaIDCh { get; set; }

		/// <summary>
		/// 	Position abbreviation
		/// </summary>
		[JsonProperty("posJobAbrvCh")]
		public string PosJobAbrvCh { get; set; }

		/// <summary>
		/// 	Physical Unit Dispatch Attach detail
		/// </summary>
		[JsonProperty("punitDispatchAttachCh")]
		public string PunitDispatchAttachCh { get; set; }

		/// <summary>
		/// 	Staffing record type = ['SCHEDULE', 'REMOVE_EXCEPTION', 'ASSIGNMENT', 'POSITION', 'EXCEPTION', 'VACANCY']
		/// </summary>
		[JsonProperty("recordType")]
		public string RecordType { get; set; }

		/// <summary>
		/// 	Region abbreviation
		/// </summary>
		[JsonProperty("regionAbrvCh")]
		public string RegionAbrvCh { get; set; }

		/// <summary>
		/// 	Region Dispatch Attach detail
		/// </summary>
		[JsonProperty("regionDispatchAttachCh")]
		public string RegionDispatchAttachCh { get; set; }

		/// <summary>
		/// 	Region externalID
		/// </summary>
		[JsonProperty("regionExternalIDCh")]
		public string RegionExternalIDCh { get; set; }

		/// <summary>
		/// 	Region id
		/// </summary>
		[JsonProperty("regionId")]
		public int RegionId { get; set; }

		/// <summary>
		/// 	Indicates whether the staffing record is removed or not
		/// </summary>
		[JsonProperty("removed")]
		public bool Removed { get; set; }

		/// <summary>
		/// 	replicated
		/// </summary>
		[JsonProperty("replicated")]
		public bool Replicated { get; set; }

		/// <summary>
		/// 	Indicates whether the staffing record is requested record or not
		/// </summary>
		[JsonProperty("request")]
		public bool Request { get; set; }

		/// <summary>
		/// 	rsc Dispatch Attach detail
		/// </summary>
		[JsonProperty("rscDispatchAttachCh")]
		public string RscDispatchAttachCh { get; set; }

		/// <summary>
		/// 	Person employeeId
		/// </summary>
		[JsonProperty("rscEmployeeIDCh")]
		public string RscEmployeeIDCh { get; set; }

		/// <summary>
		/// 	Person externalId
		/// </summary>
		[JsonProperty("rscExternalIDCh")]
		public string RscExternalIDCh { get; set; }

		/// <summary>
		/// 	Formula id assigned to person profile
		/// </summary>
		[JsonProperty("rscFormulaIDCh")]
		public string RscFormulaIDCh { get; set; }

		/// <summary>
		/// 	Person name
		/// </summary>
		[JsonProperty("rscMasterNameCh")]
		public string RscMasterNameCh { get; set; }

		/// <summary>
		/// 	Person id
		/// </summary>
		[JsonProperty("rscMasterNoIn")]
		public int RscMasterNoIn { get; set; }

		/// <summary>
		/// 	Profile id
		/// </summary>
		[JsonProperty("rscNoIn")]
		public int RscNoIn { get; set; }

		/// <summary>
		/// 	Person payrollId
		/// </summary>
		[JsonProperty("rscPayrollIDCh")]
		public string RscPayrollIDCh { get; set; }

		/// <summary>
		/// 	rscXSpecNames
		/// </summary>
		[JsonProperty("rscXSpecNames")]
		public string RscXSpecNames { get; set; }

		/// <summary>
		/// 	Shift abbreviation
		/// </summary>
		[JsonProperty("shiftAbrvCh")]
		public string ShiftAbrvCh { get; set; }

		/// <summary>
		/// 	rsc Dispatch Attach detail
		/// </summary>
		[JsonProperty("shiftDispatchAttachCh")]
		public string ShiftDispatchAttachCh { get; set; }

		/// <summary>
		/// 	shiftDurationIn
		/// </summary>
		[JsonProperty("shiftDurationIn")]
		public double ShiftDurationIn { get; set; }

		/// <summary>
		/// 	End date of the shift
		/// </summary>
		[JsonProperty("shiftEndDt")]
		public DateTime ShiftEndDt { get; set; }

		/// <summary>
		/// 	Shift id
		/// </summary>
		[JsonProperty("shiftId")]
		public int ShiftId { get; set; }

		/// <summary>
		/// 	Start date of the shift
		/// </summary>
		[JsonProperty("shiftStartDt")]
		public DateTime ShiftStartDt { get; set; }

		/// <summary>
		/// 	Account value assigned to staffing record
		/// </summary>
		[JsonProperty("staffingAccountCh")]
		public string StaffingAccountCh { get; set; }

		/// <summary>
		/// 	Calender date of the staffing record
		/// </summary>
		[JsonProperty("staffingCalendarDa")]
		public DateTime StaffingCalendarDa { get; set; }

		/// <summary>
		/// 	Created date of staffing record
		/// </summary>
		[JsonProperty("staffingCreateDateDt")]
		public DateTime StaffingCreateDateDt { get; set; }

		/// <summary>
		/// 	Staffing detail
		/// </summary>
		[JsonProperty("staffingDetailCh")]
		public string StaffingDetailCh { get; set; }

		/// <summary>
		/// 	Duration of the staffing
		/// </summary>
		[JsonProperty("staffingDurationIn")]
		public double StaffingDurationIn { get; set; }

		/// <summary>
		/// 	End date of the staffing
		/// </summary>
		[JsonProperty("staffingEndDt")]
		public DateTime StaffingEndDt { get; set; }

		/// <summary>
		/// 	Formula id assigned to staffing record
		/// </summary>
		[JsonProperty("staffingFormulaIDCh")]
		public string StaffingFormulaIDCh { get; set; }

		/// <summary>
		/// 	Staffing id
		/// </summary>
		[JsonProperty("staffingNoIn")]
		public int StaffingNoIn { get; set; }

		/// <summary>
		/// 	Staffing note
		/// </summary>
		[JsonProperty("staffingNoteVc")]
		public string StaffingNoteVc { get; set; }

		/// <summary>
		/// 	Start date of the staffing
		/// </summary>
		[JsonProperty("staffingStartDt")]
		public DateTime StaffingStartDt { get; set; }

		/// <summary>
		/// 	Station abbreviation
		/// </summary>
		[JsonProperty("stationAbrvCh")]
		public string StationAbrvCh { get; set; }

		/// <summary>
		/// 	Station Dispatch Attach detail
		/// </summary>
		[JsonProperty("stationDispatchAttachCh")]
		public string StationDispatchAttachCh { get; set; }

		/// <summary>
		/// 	Station externalID
		/// </summary>
		[JsonProperty("stationExternalIDCh")]
		public string StationExternalIDCh { get; set; }

		/// <summary>
		/// 	Unit externalID
		/// </summary>
		[JsonProperty("unitExternalIDCh")]
		public string UnitExternalIDCh { get; set; }

		/// <summary>
		/// 	Unit id
		/// </summary>
		[JsonProperty("unitId")]
		public int UnitId { get; set; }

		/// <summary>
		/// 	Work code abbreviation
		/// </summary>
		[JsonProperty("wstatAbrvCh")]
		public string WstatAbrvCh { get; set; }

		/// <summary>
		/// 	Work code id
		/// </summary>
		[JsonProperty("wstatId")]
		public int WstatId { get; set; }

		/// <summary>
		/// 	Indicates whether the staffing record is working record or not
		/// </summary>
		[JsonProperty("wstatIsWorkingGm")]
		public bool WstatIsWorkingGm { get; set; }

		/// <summary>
		/// 	Work code payroll value
		/// </summary>
		[JsonProperty("wstatPayrollCh")]
		public string WstatPayrollCh { get; set; }
    }
}
