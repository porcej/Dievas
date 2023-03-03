using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>Schedule</c> Telestaff representation of a Schedule
    /// </summary>
	public class Schedule {

		/// <summary>
		/// 	Person details
		/// </summary>
		[JsonProperty("person")]
		public SchedulePerson Person { get; set; }

		/// <summary>
		/// 	List of person schedules
		/// </summary>
		[JsonProperty("personSchedule")]
		public List<PersonSchedule> PersonSchedule { get; set; }
    }
}
