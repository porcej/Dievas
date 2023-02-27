using System;
using System.Collections.Generic;
using Dievas.Models.Telestaff;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>TSPersonScheduleRecord</c> Telestaff representation of a person schedule record
    /// </summary>
	public class TSPersonScheduleRecord {

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("blueprintExternalId")]
		public string BlueprintExternalId { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("blueprintJobAbbreviation")]
		public string BlueprintJobAbbreviation { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("blueprintJobAccount")]
		public string BlueprintJobAccount { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("blueprintJobExternalId")]
		public string BlueprintJobExternalId { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue(0)]
		[JsonProperty("blueprintJobGroups")]
		public List<string> BlueprintJobGroups { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue(0)]
		[JsonProperty("blueprintJobSpecialities")]
		public List<string> BlueprintJobSpecialities { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue(0)]
		[JsonProperty("blueprintLabels")]
		public List<string> BlueprintLabels { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("blueprintName")]
		public string BlueprintName { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("blueprintOrderInformation")]
		public string BlueprintOrderInformation { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue(0)]
		[JsonProperty("blueprintQuantityMax")]
		public int BlueprintQuantityMax { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue(0)]
		[JsonProperty("blueprintQuantityMin")]
		public int BlueprintQuantityMin { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("blueprintSKU")]
		public string BlueprintSKU { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue(0)]
		[JsonProperty("durationInHours")]
		public int DurationInHours { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue(0)]
		[JsonProperty("durationInSecs")]
		public int DurationInSecs { get; set; }
		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("endDate")]
		public string EndDate { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("endTime")]
		public string EndTime { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("note")]
		public string Note { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue(0)]
		[JsonProperty("payrollDurationInHours")]
		public int PayrollDurationInHours { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue(0)]
		[JsonProperty("payrollDurationInSecs")]
		public int PayrollDurationInSecs { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("staffingAccount")]
		public string StaffingAccount { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("staffingFormulaId")]
		public string StaffingFormulaId { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue(0)]
		[JsonProperty("staffingNoIn")]
		public int StaffingNoIn { get; set; }
		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("startDate")]
		public string StartDate { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("startTime")]
		public string StartTime { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("tagName")]
		public string TagName { get; set; }

		/// <summary>
		///   
		/// </summary>
		[DefaultValue(false)]
		[JsonProperty("noRosterImpact")]
		public bool NoRosterImpact { get; set; }

		/// <summary>
		///   
		/// </summary>
		[JsonProperty("costCenter")]
		public TSCostCenter CostCenter { get; set; }

		/// <summary>
		///   
		/// </summary>
		[JsonProperty("detailCode")]
		public TSDetailCode DetailCode { get; set; }

		/// <summary>
		///   
		/// </summary>
		[JsonProperty("laborCategories")]
		public List<TSLaborCategory> LaborCategories { get; set; }

		/// <summary>
		///   
		/// </summary>
		[JsonProperty("organization")]
		public TSOrganization Organization { get; set; }

		// /// <summary>
		// ///   
		// /// </summary>
		// [JsonProperty("profile")]
		// public TSProfile Profile { get; set; }

		// /// <summary>
		// ///   
		// /// </summary>
		// [JsonProperty("shift")]
		// public TSShift Shift { get; set; }

		// /// <summary>
		// ///   
		// /// </summary>
		// [JsonProperty("workCode")]
		// public TSWorkCode WorkCode { get; set; }
    }
}
