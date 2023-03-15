using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>WorkCodeBehavior</c> Telestaff  properties that allows to configure the work code behaviour i.e By adding approval,detail code,account,start time,end time and others 
    /// </summary>
	public class WorkCodeBehavior {

		/// <summary>
		/// 	Allows for additional information to be entered into the staffing record as the Work Code is being added to a Roster.
		/// </summary>
		[JsonProperty("account")]
		public string Account { get; set; }

		/// <summary>
		/// 	Approval options for the Work Code.
		/// </summary>
		[JsonProperty("approval")]
		public string Approval { get; set; }

		/// <summary>
		/// 	Sets this Work Code to a default to a time frame. Placing a checkmark in this option changes the Earliest Start Time and Latest End Time options to Default Start Time and Default End Time.
		/// </summary>
		[JsonProperty("defaultTimeMode")]
		public bool DefaultTimeMode { get; set; }

		/// <summary>
		/// 	Allows for additional information to entered into the staffing record as the Work Code is being added to a Roster.
		/// </summary>
		[JsonProperty("detailCode")]
		public NameAndId DetailCode { get; set; }

		/// <summary>
		/// 	Defines a start time for this Work Code to be used. This Work Code cannot be used with a Start Time prior to the time entered in this field.
		/// </summary>
		[JsonProperty("earliestStartTime")]
		public string EarliestStartTime { get; set; }

		/// <summary>
		/// 	Defines an end time for this Work Code to be used.
		/// </summary>
		[JsonProperty("latestEndTime")]
		public string LatestEndTime { get; set; }

		/// <summary>
		/// 	All vacancies caused by the selected Work Code will be filled using the selected List plan.
		/// </summary>
		[JsonProperty("listPlanOverride")]
		public string ListPlanOverride { get; set; }

		/// <summary>
		/// 	The measurement mode to track trade paybacks between two users.
		/// </summary>
		[JsonProperty("paybacksBy")]
		public string PaybacksBy { get; set; }

		/// <summary>
		/// 	Allows preference for use of a Work Code on a specific Unit rather than for a day or time period.
		/// </summary>
		[JsonProperty("where")]
		public string Where { get; set; }
    }
}
