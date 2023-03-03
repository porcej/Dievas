using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>WorkCodeCover</c> Telestaff Cover person work code properties
    /// </summary>
	public class WorkCodeCover {

		/// <summary>
		/// 	To indicate this Work Code is a cover type Work Code and requires a replacement person with the required assignment
		/// </summary>
		[JsonProperty("assignmentRequired")]
		public bool AssignmentRequired { get; set; }

		/// <summary>
		/// 	Listing of all the possible assignment requirements for the replacement person
		/// </summary>
		[JsonProperty("assignmentRequirement")]
		public string AssignmentRequirement { get; set; }

		/// <summary>
		/// 	List of work code for the Cover person selected.
		/// </summary>
		[JsonProperty("coverWorkCode")]
		public List<string> CoverWorkCode { get; set; }

		/// <summary>
		/// 	The maximum amount of time in minutes that two Shifts may overlap when performing a Shift trade.
		/// </summary>
		[JsonProperty("overlapThreshold")]
		public int OverlapThreshold { get; set; }

		/// <summary>
		/// 	Work Code to be added when overlap exists during the specified threshold.
		/// </summary>
		[JsonProperty("overlapWorkCode")]
		public string OverlapWorkCode { get; set; }
    }
}
