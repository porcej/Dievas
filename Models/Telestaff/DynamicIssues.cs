using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>DynamicIssues</c> Telestaff issue that typically designed to stop the usage of a Work Code based on certain parameters (business rules)
    /// </summary>
	public class DynamicIssues {

		/// <summary>
		/// 	Dynamic issue name
		/// </summary>
		[JsonProperty("dynamicIssue")]
		public string DynamicIssue { get; set; }

		/// <summary>
		/// 	Status of Dynamic issue.Can be Applies/Does Not Apply/Contributes to group
		/// </summary>
		[JsonProperty("status")]
		public string Status { get; set; }
    }
}