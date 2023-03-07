using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>PersonSchedule</c> Telestaff representation of a schedule for a specific person
    /// </summary>
	public class PersonSchedule {

		/// <summary>
		/// 	Blueprint external system Id ,
		/// </summary>
		[JsonProperty("blueprintExternalId")]
		public string BlueprintExternalId { get; set; }

		/// <summary>
		/// 	Blueprint job abbreviation ,
		/// </summary>
		[JsonProperty("blueprintJobAbbreviation")]
		public string BlueprintJobAbbreviation { get; set; }

		/// <summary>
		/// 	Blueprint job account ,
		/// </summary>
		[JsonProperty("blueprintJobAccount")]
		public string BlueprintJobAccount { get; set; }

		/// <summary>
		/// 	Blueprint job external system Id ,
		/// </summary>
		[JsonProperty("blueprintJobExternalId")]
		public string BlueprintJobExternalId { get; set; }

		/// <summary>
		/// 	List of job groups assigned to the blueprint ,
		/// </summary>
		[JsonProperty("blueprintJobGroups")]
		public List<string> BlueprintJobGroups { get; set; }

		/// <summary>
		/// 	List of specialities assigned to the blueprint ,
		/// </summary>
		[JsonProperty("blueprintJobSpecialities")]
		public List<string> BlueprintJobSpecialities { get; set; }

		/// <summary>
		/// 	List of blueprint labels ,
		/// </summary>
		[JsonProperty("blueprintLabels")]
		public List<string> BlueprintLabels { get; set; }

		/// <summary>
		/// 	Name of the blueprint ,
		/// </summary>
		[JsonProperty("blueprintName")]
		public string BlueprintName { get; set; }

		/// <summary>
		/// 	Blueprint Order Information ,
		/// </summary>
		[JsonProperty("blueprintOrderInformation")]
		public string BlueprintOrderInformation { get; set; }

		/// <summary>
		/// 	
		/// </summary>
		[JsonProperty("blueprintQuantityMax")]
		public int BlueprintQuantityMax { get; set; }

		/// <summary>
		/// 	
		/// </summary>
		[JsonProperty("blueprintQuantityMin")]
		public int BlueprintQuantityMin { get; set; }

		/// <summary>
		/// 	SKU code of blueprint ,
		/// </summary>
		[JsonProperty("blueprintSKU")]
		public string BlueprintSKU { get; set; }

		/// <summary>
		/// 	CostCenter details ,
		/// </summary>
		[JsonProperty("costCenter")]
		public CostCenter CostCenter { get; set; }

		/// <summary>
		/// 	Detail code details ,
		/// </summary>
		[JsonProperty("detailCode")]
		public DetailCode DetailCode { get; set; }

		/// <summary>
		/// 	Schedule duration in hours ,
		/// </summary>
		[JsonProperty("durationInHours")]
		public double DurationInHours { get; set; }

		/// <summary>
		/// 	Schedule duration in seconds ,
		/// </summary>
		[JsonProperty("durationInSecs")]
		public int DurationInSecs { get; set; }

		/// <summary>
		/// 	Schedule end date ,
		/// </summary>
		[JsonProperty("endDate")]
		public DateTime EndDate { get; set; }

		/// <summary>
		/// 	Schedule end time ,
		/// </summary>
		[JsonProperty("endTime")]
		public DateTime EndTime { get; set; }

		/// <summary>
		/// 	labor categories data ,
		/// </summary>
		[JsonProperty("laborCategories")]
		public List<LaborCategory> LaborCategories { get; set; }

		/// <summary>
		/// 	No Roster Impact flag for staffing records ,
		/// </summary>
		[JsonProperty("noRosterImpact")]
		public bool NoRosterImpact { get; set; }

		/// <summary>
		/// 	Staffing note ,
		/// </summary>
		[JsonProperty("note")]
		public string Note { get; set; }

		/// <summary>
		/// 	Organization details ,
		/// </summary>
		[JsonProperty("organization")]
		public Organization Organization { get; set; }

		/// <summary>
		/// 	Schedule payroll duration in hours ,
		/// </summary>
		[JsonProperty("payrollDurationInHours")]
		public double PayrollDurationInHours { get; set; }

		/// <summary>
		/// 	Schedule payroll duration in seconds ,
		/// </summary>
		[JsonProperty("payrollDurationInSecs")]
		public int PayrollDurationInSecs { get; set; }

		/// <summary>
		/// 	Profile details ,
		/// </summary>
		[JsonProperty("profile")]
		public Profile Profile { get; set; }

		/// <summary>
		/// 	Shift details ,
		/// </summary>
		[JsonProperty("shift")]
		public Shift Shift { get; set; }

		/// <summary>
		/// 	Staffing account value ,
		/// </summary>
		[JsonProperty("staffingAccount")]
		public string StaffingAccount { get; set; }

		/// <summary>
		/// 	Evaluted position formula id value ,
		/// </summary>
		[JsonProperty("staffingFormulaId")]
		public string StaffingFormulaId { get; set; }

		/// <summary>
		/// 	Staffing id ,
		/// </summary>
		[JsonProperty("staffingNoIn")]
		public int StaffingNoIn { get; set; }

		/// <summary>
		/// 	Schedule start date ,
		/// </summary>
		[JsonProperty("startDate")]
		public DateTime StartDate { get; set; }

		/// <summary>
		/// 	Schedule start time ,
		/// </summary>
		[JsonProperty("startTime")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// 	Tag Name ,
		/// </summary>
		[JsonProperty("tagName")]
		public string TagName { get; set; }

		/// <summary>
		/// 	Work code details
		/// </summary>
		[JsonProperty("workCode")]
		public WorkCode WorkCode { get; set; }
    }
}
