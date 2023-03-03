using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>Roster</c> Telestaff representation of a roster
    /// </summary>
	public class Roster {

		/// <summary>
		/// 	Roster Date
		/// </summary>
		[JsonProperty("date")]
		public DateTime Date { get; set; }

		/// <summary>
		/// 	List of roster records on date
		/// </summary>
		[JsonProperty("records")]
		public List<RosterRecord> records { get; set; }
    }
}
