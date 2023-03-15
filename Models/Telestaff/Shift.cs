using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>Shift</c> Telestaff representation of the details for a specific shift
    /// </summary>
	public class Shift {

		/// <summary>
		/// 	Abbreviation of entity
		/// </summary>
		[JsonProperty("abbreviation")]
		public string Abbreviation { get; set; }

		/// <summary>
		/// 	Attachments for the shift for that shift calender day
		/// </summary>
		[JsonProperty("attachments")]
		public List<string> Attachments { get; set; }

		/// <summary>
		/// 	entity is disabled or not
		/// </summary>
		[JsonProperty("disabled")]
		public bool Disabled { get; set; }

		/// <summary>
		/// 	Shift end time for the calendar day if shift is active on that day
		/// </summary>
		[JsonProperty("endTime")]
		public DateTime EndTime { get; set; }

		/// <summary>
		/// 	Internal identifier of entity
		/// </summary>
		[JsonProperty("id")]
		public int Id { get; set; }

		/// <summary>
		/// 	Name of entity
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// 	Shift start time for the calendar day if shift is active on that day
		/// </summary>
		[JsonProperty("startTime")]
		public DateTime StartTime { get; set; }

    }
}
