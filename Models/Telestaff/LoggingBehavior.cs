using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>LoggingBehavior</c> Telestaff Logging Behavior
    /// </summary>
	public class LoggingBehavior {

		/// <summary>
		/// 	Type of behaviour
		/// </summary>
		[JsonProperty("behavior")]
		public string Behavior { get; set; }

		/// <summary>
		/// 	To designate whether this code should be logged
		/// </summary>
		[JsonProperty("status")]
		public string Status { get; set; }
    }
}
