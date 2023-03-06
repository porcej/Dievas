using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Festis {

   	/// <summary>
    ///     Class <c>StaffingShift</c> Staffing information for a specific shift generrated by <see href="https://github.com/porcej/festis">Festis</see>
    /// </summary>
	public class StaffingShift: StaffingNode {

		/// <summary>
		/// 	List of station Nodes
		/// </summary>
		[JsonProperty("Station")]
		public List<StaffingStation> Station { get; set; }
    }
}
