using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>SchedulePerson</c> Telestaff representation of a person
    /// </summary>
	public class SchedulePerson {

		/// <summary>
		/// 	Attachments for the person for that calender day
		/// </summary>
		[JsonProperty("attachments")]
		public List<string> Attachments { get; set; }

		/// <summary>
		/// 	Person Employee Id
		/// </summary>
		[JsonProperty("employeeId")]
		public string EmployeeId { get; set; }

		/// <summary>
		/// 	Person External Id
		/// </summary>
		[JsonProperty("externalId")]
		public string ExternalId { get; set; }

		/// <summary>
		/// 	Person Telestaff ID
		/// </summary>
		[JsonProperty("id")]
		public int Id { get; set; }

		/// <summary>
		/// 	Name of the person
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// 	Person Payroll Id
		/// </summary>
		[JsonProperty("payrollId")]
		public string PayrollId { get; set; }
    }
}
