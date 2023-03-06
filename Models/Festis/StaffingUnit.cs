using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Festis {

   	/// <summary>
    ///     Class <c>StaffingUnit</c> Staffing information for a specific unit generrated by <see href="https://github.com/porcej/festis">Festis</see>
    /// </summary>
	public class StaffingUnit: StaffingNode {

		/// <summary>
		/// 	List of Postions
		/// </summary>
		[JsonProperty("Postion")]
		public List<StaffingPosition> Postion { get; set; }
    }
}
