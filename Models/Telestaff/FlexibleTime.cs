using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>FlexibleTime</c> Telestaff definition for flexibility in shift assignment and shift assignment vacancy
    /// </summary>
	public class FlexibleTime {

		/// <summary>
		/// 	Minimum percentage that the Work Code's length must match in order for the modification of the Assignment to occur
		/// </summary>
		[JsonProperty("assignmentDurationMinPercentage")]
		public float AssignmentDurationMinPercentage { get; set; }

		/// <summary>
		/// 	Percentage that the Work Code must overlap the Assignment in order for the modification of the Assignment to occur
		/// </summary>
		[JsonProperty("assignmentOverlapMinimumPercentage")]
		public float AssignmentOverlapMinimumPercentage { get; set; }

		/// <summary>
		/// 	To perform the overlap Assignment check in order to determine the default state of the suppress Assignments
		/// </summary>
		[JsonProperty("enableAssignmentOverlapCheck")]
		public bool EnableAssignmentOverlapCheck { get; set; }

		/// <summary>
		/// 	To suppress the shift Assignment using Work Code
		/// </summary>
		[JsonProperty("suppressAssignment")]
		public bool SuppressAssignment { get; set; }

		/// <summary>
		/// 	To automatically suppress the shift assignment vacancy on the Roster when applying a nonworking type Work Code to this Work Code
		/// </summary>
		[JsonProperty("suppressAssignmentVacancy")]
		public bool SuppressAssignmentVacancy { get; set; }
    }
}
