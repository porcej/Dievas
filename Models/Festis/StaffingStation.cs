using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Festis {

   	/// <summary>
    ///     Class <c>StaffingStation</c> Staffing information for a specific station generrated by <see href="https://github.com/porcej/festis">Festis</see>
    /// </summary>
	public class StaffingStation: StaffingNode {

		/// <summary>
		/// 	List of Shift Nodes
		/// </summary>
		[JsonProperty("Station")]
		public List<StaffingStation> Station { get; set; }
    }
}
