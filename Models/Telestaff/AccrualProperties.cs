using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>AccrualProperties</c> Telestaff representation of the properties for an acrrual
    /// </summary>
	public class AccrualProperties {

		/// <summary>
		/// 	Defines the number of hours/counts that can transfer from one period to the next
		/// </summary>
		[JsonProperty("carry")]
		public double Carry { get; set; }

		/// <summary>
		/// 	The default effective date used when calculating accrual balances
		/// </summary>
		[JsonProperty("effectiveDa")]
		public string EffectiveDa { get; set; }

		/// <summary>
		/// 	The Accrual Group number is used primarily to track selected Work Code s and their related accrual numbers
		/// </summary>
		[JsonProperty("group")]
		public int Group { get; set; }

		/// <summary>
		/// 	This field is only valid for working codes. Defines the maximum amount of hours/counts per person allowed in this Accrual Group
		/// </summary>
		[JsonProperty("minMax")]
		public double MinMax { get; set; }

		/// <summary>
		/// 	Defines when an Accrual Balance starts. Any records prior to the given date are ignored
		/// </summary>
		[JsonProperty("periodMonths")]
		public int PeriodMonths { get; set; }

		/// <summary>
		/// 	The rate at which to accrue either Count or Hours for the selected Work Code
		/// </summary>
		[JsonProperty("rate")]
		public double Rate { get; set; }

		/// <summary>
		/// 	Indicates if unapproved requests records should be considered part of the accrual balance for this Work Code
		/// </summary>
		[JsonProperty("requestsToo")]
		public string RequestsToo { get; set; }

		/// <summary>
		/// 	This is a date field that determines when an Accrual Balance starts. Any records prior to the given date are ignored
		/// </summary>
		[JsonProperty("sinceDa")]
		public string SinceDa { get; set; }

		/// <summary>
		/// 	The Type is the Unit of measure for the accrual balance
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; set; }
    }
}
