using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>TSPerson</c> Telestaff representation of a person
    /// </summary>
	public class TSPerson {

		/// <summary>
		/// 	TS Person Record ID
		/// </summary>
		[JsonProperty("attachments")]
		public List<string> Attachments { get; set; }

		/// <summary>
		/// 	TS Employee ID
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("employeeId")]
		public string employeeId { get; set; }

		/// <summary>
		/// 	TS External ID
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("externalId")]
		public string externalId { get; set; }

		/// <summary>
		/// 	TS Person Unique ID
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("id")]
		public int id { get; set; }

		/// <summary>
		/// 	Name: Last, First
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("name")]
		public string name { get; set; }

		/// <summary>
		/// 	TS Payroll ID
		/// </summary>
		[DefaultValue("")]
		[JsonProperty("payrollId")]
		public string payrollId { get; set; }
    }
}
