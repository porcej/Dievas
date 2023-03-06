using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Festis {

   	/// <summary>
    ///     Class <c>StaffingInstitution</c> Staffing information for a specific institution organization node generrated by <see href="https://github.com/porcej/festis">Festis</see>
    /// </summary>
	public class StaffingInstitution: StaffingNode {

		/// <summary>
		/// 	List of Agency nodes
		/// </summary>
		[JsonProperty("Agency")]
		public List<StaffingAgency> Agency { get; set; }
		
    }
}
