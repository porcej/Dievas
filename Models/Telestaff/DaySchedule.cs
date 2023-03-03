using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>DaySchedule</c> Telestaff representation all the schedules in a day
    /// </summary>
	public class DaySchedule {

		/// <summary>
		/// 	Date details
		/// </summary>
		[JsonProperty("date")]
		public DateTime date { get; set; }

		/// <summary>
		/// 	List of schedules for that day
		/// </summary>
		[JsonProperty("schedule")]
		public List<Schedule> Schedule { get; set; }
    }
}
