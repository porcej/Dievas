using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Festis {

   	/// <summary>
    ///     Class <c>StaffingBatallion</c> Staffing information for a specific batallion organization node generrated by <see href="https://github.com/porcej/festis">Festis</see>
    /// </summary>
	public class StaffingBatallion: StaffingNode {

		/// <summary>
		/// 	List of Batallion Nodes
		/// </summary>
		[JsonProperty("Batallion")]
		public List<StaffingShift> Shift { get; set; }
		
    }
}
