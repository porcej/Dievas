using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

    /// <summary>
    ///     Class <c>TSRosterRecord</c> represents a single record from a Telestaff Roster
    /// </summary>
	public class TSRosterRecord {

		/// <summary>
		/// 	TS Person Record ID
		/// </summary>
		[JsonProperty("rscMasterNoIn")]
		public int RscMasterNoIn { get; set; }

		/// <summary>
		/// 	Name: Last, First
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("rscMasterNameCh")]
		public string RscMasterNameCh { get; set; }

		/// <summary>
		/// 	Badge Number
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("rscEmployeeIDCh")]
		public string RscEmployeeIDCh { get; set; }

		/// <summary>
		/// 	Payroll ID
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("rscPayrollIDCh")]
		public string RscPayrollIDCh { get; set; }

		/// <summary>
		/// 	Shift short-code <example>"Adm"</example>
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("shiftAbrvCh")]
		public string ShiftAbrvCh { get; set; }

		/// <summary>
		/// 	Institution Short code <example>"AFD"</example>
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("institutionAbrvCh")]
		public string InstitutionAbrvCh { get; set; }

		/// <summary>
		/// 	Agency short code <example>"Alex FD"</example>
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("agencyAbrvCh")]
		public string AgencyAbrvCh { get; set; }

		/// <summary>
		/// 	Region <example>"Chief"</example>
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("regionAbrvCh")]
		public string RegionAbrvCh { get; set; }

		/// <summary>
		/// 	Station Name <example>"Admin OFC"</example>
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("stationAbrvCh")]
		public string StationAbrvCh { get; set; }

		/// <summary>
		/// 	Physical Unit Name <example>"Fire Chief"</example>
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("physicalUnitAbrvCh")]
		public string PhysicalUnitAbrvCh { get; set; }

		/// <summary>
		/// 	Position Short Code <example>".CHF"</example>
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("posJobAbrvCh")]
		public string PosJobAbrvCh { get; set; }

		/// <summary>
		/// 	Staffing Duration <example>8.0</example>
		/// </summary>
		[JsonProperty("staffingDurationIn")]
		public float StaffingDurationIn { get; set; }

		/// <summary>
		/// 	Staffing Start Datetime <example>"2023-02-27T08:00:00"</example>
		/// </summary>
		[JsonProperty("staffingStartDt")]
		public DateTime StaffingStartDt { get; set; }

		/// <summary>
		/// 	staffing End DateTime <example>"2023-02-27T16:00:00"</example>
		/// </summary>
		[JsonProperty("staffingEndDt")]
		public DateTime StaffingEndDt { get; set; }

		/// <summary>
		/// 	Order to show on roster <example>1</example>
		/// </summary>
		[JsonProperty("displayOrderGm")]
		public int DisplayOrderGm { get; set; }
        [DefaultValue("")]

		/// <summary>
		/// 	Detail Code <example>"dtAOF"</example>
        /// </summary>
		[JsonProperty("staffingDetailCh")]
		public string StaffingDetailCh { get; set; }
        [DefaultValue("")]

		/// <summary>
		/// 	Notes related to staffing
        /// </summary>
		[JsonProperty("staffingNoteVc")]
		public string StaffingNoteVc { get; set; }
        [DefaultValue("")]

		/// <summary>
		/// 	Unknown text field
        /// </summary>
		[JsonProperty("rscDispatchAttachCh")]
		public string RscDispatchAttachCh { get; set; }
        [DefaultValue("")]

		/// <summary>
		/// 	Notes attached to assigned unit
        /// </summary>
		[JsonProperty("punitDispatchAttachCh")]
		public string PunitDispatchAttachCh { get; set; }
        [DefaultValue("")]

		/// <summary>
		/// 	Working short codes <example>"ANL"</example>
        /// </summary>
		[JsonProperty("wstatAbrvCh")]
		public string WstatAbrvCh { get; set; }

		/// <summary>
		/// 	Working code for payroll <example>"REG"</example>
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("wstatPayrollCh")]
		public string WstatPayrollCh { get; set; }

		/// <summary>
		/// 	shiftStartDt <example>"2023-02-27T08:00:00"</example>
		/// </summary>
		[JsonProperty("shiftStartDt")]
		public DateTime ShiftStartDt { get; set; }

		/// <summary>
		/// 	Shift End Datetime <example>"2023-02-27T16:00:00"</example>
		/// </summary>
		[JsonProperty("shiftEndDt")]
		public DateTime ShiftEndDt { get; set; }
        [DefaultValue("")]

		/// <summary>
		/// 	Specialities <example>".TR,BA,DOE,DOT,FBC,He,IWB,TN"</example>
		/// </summary>
		[JsonProperty("rscXSpecNames")]
		public string RscXSpecNames { get; set; }

		/// <summary>
		/// 	Full assigned unit name <example>"Fire Chief"</example>
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("physicalUnitNameCh")]
		public string PhysicalUnitNameCh { get; set; }

		/// <summary>
		/// 	Record Type <example>"ASSIGNMENT"</example>
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("recordType")]
		public string RecordType { get; set; }

		/// <summary>
		/// 	removed flag <example>false</example>
		/// </summary>
		[JsonProperty("removed")]
		public bool Removed { get; set; }

		/// <summary>
		/// 	request flag <example>false</example>
		/// </summary>
		[JsonProperty("request")]
		public bool Request { get; set; }

		/// <summary>
		/// 	Overall person NAME ID <example>2618</example>
		/// </summary>
		[JsonProperty("rscNoIn")]
		public int RscNoIn { get; set; }

		/// <summary>
		/// 	staffing Calendar datetime <example>"2023-02-27T00:00:00"</example>
		/// </summary>
		[JsonProperty("staffingCalendarDa")]
		public DateTime StaffingCalendarDa { get; set; }

		/// <summary>
		/// 	Work code id <example>1</example>
		/// </summary>
		[JsonProperty("wstatId")]
		public int WstatId { get; set; }

		/// <summary>
		/// 	pay Duration  <example>8.0</example>
		/// </summary>
		[JsonProperty("payDurationIn")]
		public flaot PayDurationIn { get; set; }

		/// <summary>
		/// 	shift Duration <example>8.0</example>
		/// </summary>
		[JsonProperty("shiftDurationIn")]
		public float ShiftDurationIn { get; set; }

		/// <summary>
		/// 	replicated flag <example>false</example>
		/// </summary>
		[JsonProperty("replicated")]
		public bool Replicated { get; set; }

		/// <summary>
		/// 	TS Shift ID <example>5</example>
		/// </summary>
		[JsonProperty("shiftId")]
		public int ShiftId { get; set; }

		/// <summary>
		/// 	TS Institution ID <example>1</example>
		/// </summary>
		[JsonProperty("institutionId")]
		public int InstitutionId { get; set; }

		/// <summary>
		/// 	TS Region ID <example>13</example>
		/// </summary>
		[JsonProperty("regionId")]
		public int RegionId { get; set; }

		/// <summary>
		/// 	TS Unit ID <example>157
		/// </summary>
		[JsonProperty("unitId")]
		public int UnitId { get; set; }
    }
}
