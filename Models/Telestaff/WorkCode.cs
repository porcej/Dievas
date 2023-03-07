using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>WorkCode</c> Telestaff representation of the details for a Work Code
    /// </summary>
	public class WorkCode {

		/// <summary>
		/// 	Abbreviation
		/// </summary>
		[JsonProperty("abrv")]
		public string Abrv { get; set; }

		/// <summary>
		/// 	Stores additional payroll data such as the cost center or project double paying for this staffing record
		/// </summary>
		[JsonProperty("account")]
		public string Account { get; set; }

		/// <summary>
		/// 	Allows to configure the accrual attributes for this work code
		/// </summary>
		[JsonProperty("accrualProperties")]
		public AccrualProperties AccrualProperties { get; set; }

		/// <summary>
		/// 	Separate Work Codes into groupings or classifications
		/// </summary>
		[JsonProperty("category")]
		public string Category { get; set; }

		/// <summary>
		/// 	Whether or not the Work Code is disabled
		/// </summary>
		[JsonProperty("disable")]
		public bool Disable { get; set; }

		/// <summary>
		/// 	Defines flexibility in shift assignment and shift assignment vacancy
		/// </summary>
		[JsonProperty("flexibleTime")]
		public FlexibleTime FlexibleTime { get; set; }

		/// <summary>
		/// 	The Accrual Group double is used primarily to track selected Work Codes and their related accrual doubles
		/// </summary>
		[JsonProperty("group")]
		public int Group { get; set; }

		/// <summary>
		/// 	id
		/// </summary>
		[JsonProperty("id")]
		public int Id { get; set; }

		/// <summary>
		/// 	Indicates that Work Code is included in integration
		/// </summary>
		[JsonProperty("includeInIntegration")]
		public bool IncludeInIntegration { get; set; }

		/// <summary>
		/// 	List of issues that are typically designed to stop the usage of a Work Code based on certain parameters (business rules)
		/// </summary>
		[JsonProperty("issues")]
		public List<DynamicIssues> Issues { get; set; }

		/// <summary>
		/// 	List of all Logging Behaviors
		/// </summary>
		[JsonProperty("loggingBehaviors")]
		public List<LoggingBehavior> LoggingBehaviors { get; set; }

		/// <summary>
		/// 	Defines a maximum hours limit for the Work Code. The Work Code cannot be used unless the hours specified are equal to or less than the amount of hours entered in this field
		/// </summary>
		[JsonProperty("maxHours")]
		public double MaxHours { get; set; }

		/// <summary>
		/// 	Defines a minimum hours limit for the Work Code. The Work Code cannot be used unless the hours specified are equal to or greater than the amount of hours entered in this field
		/// </summary>
		[JsonProperty("minHours")]
		public double MinHours { get; set; }

		/// <summary>
		/// 	name
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// 	Indicates that Work Code is of type on call
		/// </summary>
		[JsonProperty("onCall")]
		public bool OnCall { get; set; }

		/// <summary>
		/// 	Required for working and non-working Work Code to be available for feeding a payroll system or pass the Work Code to the Payroll, FLSA, and the Exceptions report; if this field is left blank, then the selected Work Code is not passed to these reports
		/// </summary>
		[JsonProperty("payrollCode")]
		public string PayrollCode { get; set; }

		/// <summary>
		/// 	If true, this Work Code is sent as Pay Code Edit
		/// </summary>
		[JsonProperty("sendAsPayCodeEdit")]
		public bool SendAsPayCodeEdit { get; set; }

		/// <summary>
		/// 	Work Code type defines behaviour of work code
		/// </summary>
		[JsonProperty("sendAsSegmentWithTag")]
		public bool SendAsSegmentWithTag { get; set; }

		/// <summary>
		/// 	Defines the conditions for suppressing the Work Code. If Yes: Suppresses any related vacancies on the Roster. If No: Does not suppress a vacancy on the Roster. If Dynamic: The vacancy suppression is determined by the Vacancy Strategy (List Plan)
		/// </summary>
		[JsonProperty("suppressVacancy")]
		public string SuppressVacancy { get; set; }

		/// <summary>
		/// 	Defines cost information for the selected Work Code and used on the Cost Report when Wage information for the Person.
		/// </summary>
		[JsonProperty("wageFactor")]
		public double WageFactor { get; set; }

		/// <summary>
		/// 	Additional properties that allows to configure the work code behaviour i.e By adding approval,detail code,account,start time,end time and others
		/// </summary>
		[JsonProperty("workCodeBehavior")]
		public WorkCodeBehavior WorkCodeBehavior { get; set; }

		/// <summary>
		/// 	Cover person work code properties
		/// </summary>
		[JsonProperty("workCodeCover")]
		public WorkCodeCover WorkCodeCover { get; set; }

		/// <summary>
		/// 	Work Code type defines behaviour of work code. It can be WORKING/NON-WORKING/REGULAR DUTY/SIGN UP
		/// </summary>
		[JsonProperty("workCodeType")]
		public NameAndId WorkCodeType { get; set; }
    }
}
