using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>PositionNode</c> extends <c>OrganizationNode</c> Telestaff representation of an position
    /// </summary>
	public class PositionNode: OrganizationNode {

		/// <summary>
		/// 	Job Title for this Node
		/// </summary>
		[JsonProperty("jobTitle")]
		public JobTitle JobTitle { get; set; }
    }
}
