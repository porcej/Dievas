using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Festis {

   	/// <summary>
    ///     Class <c>StaffingRoster</c> Staffing information for a roster as generrated by <see href="https://github.com/porcej/festis">Festis</see>
    /// </summary>
	public class StaffingRoster: StaffingNode {

		/// <summary>
		/// 	List of Institution nodes
		/// </summary>
		[JsonProperty("Institution")]
		public List<StaffingInstitution> Institution { get; set; }
		
    }
}
