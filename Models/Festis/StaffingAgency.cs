using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Festis {

   	/// <summary>
    ///     Class <c>StaffingAgency</c> Staffing information for a specific agency organization node generrated by <see href="https://github.com/porcej/festis">Festis</see>
    /// </summary>
	public class StaffingAgency: StaffingNode {

		/// <summary>
		/// 	List of Batallions
		/// </summary>
		[JsonProperty("Batallion")]
		public List<StaffingBatallion> Batallion { get; set; }
    }
}
