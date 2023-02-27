using System;
using System.Collections.Generic;
using Dievas.Models.Telestaff;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

    /// <summary>
    ///     Class <c>TSScheduleRecord</c> represents a single record from a Telestaff Schedule
    /// </summary>
	public class TSScheduleRecord {

		/// <summary>
		/// 	Person information for this record
		/// </summary>
		[JsonProperty("person")]
		public TSPerson Person { get; set; }

		/// <summary>
		/// 	Time slot information of this record
		/// </summary>
		[JsonProperty("personSchedule")]
		public List<TSPersonSchedule> PersonSchedule { get; set; }
    }
}
